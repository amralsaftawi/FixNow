using System.Net;
using System.Net.Http.Json;
using FixNow.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FixNow.Infrastructure.Services;

public sealed class EmailOtpSender(
    IConfiguration configuration,
    ILogger<EmailOtpSender> logger) : IOtpSender
{
    private const string BrevoEmailApiUrl = "https://api.brevo.com/v3/smtp/email";

    private const int ExpirationMinutes = 5;

    private static readonly HttpClient HttpClient = new();

    private static readonly Error SendFailed =
        Error.Failure(
            code: "Identity.EmailOtp.SendFailed",
            description: "The verification email could not be sent.");

    private static readonly Error ConfigurationMissing =
        Error.Unexpected(
            code: "Identity.EmailOtp.ConfigurationMissing",
            description: "The email delivery service is not configured.");

    private static readonly Error PhoneDeliveryNotSupported =
        Error.Validation(
            code: "Identity.Otp.PhoneDeliveryNotSupported",
            description: "Phone OTP delivery is not supported yet.");

    public async Task<Result<Success>> SendAsync(
        User user,
        string otp,
        OtpPurpose purpose,
        CancellationToken cancellationToken)
    {
        var email = ResolveEmail(user, purpose);

        if (email is null)
        {
            return purpose switch
            {
                OtpPurpose.PhoneVerification or
                OtpPurpose.ChangePhoneNumber or
                OtpPurpose.LoginVerification => PhoneDeliveryNotSupported,
                _ => SendFailed
            };
        }

        var apiKey = configuration["Brevo:ApiKey"];
        var senderEmail = configuration["Brevo:SenderEmail"];
        var senderName = configuration["Brevo:SenderName"];

        if (string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(senderEmail) ||
            string.IsNullOrWhiteSpace(senderName))
        {
            return ConfigurationMissing;
        }

        var payload = new
        {
            sender = new
            {
                email = senderEmail,
                name = senderName
            },
            to = new[]
            {
                new { email }
            },
            subject = "FixNow - Your verification code",
            htmlContent = BuildHtmlContent(otp, purpose)
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            BrevoEmailApiUrl);

        request.Headers.Add("api-key", apiKey);
        request.Headers.Add("Accept", "application/json");
        request.Content = JsonContent.Create(payload);

        try
        {
            using var response = await HttpClient.SendAsync(
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                logger.LogWarning(
                    "Failed to send OTP email. Brevo responded with status {StatusCode}: {ErrorBody}",
                    (int)response.StatusCode,
                    errorBody);

                return SendFailed;
            }

            logger.LogInformation("OTP email sent.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to send OTP email.");

            return SendFailed;
        }

        return Result.Success;
    }

    private static string? ResolveEmail(User user, OtpPurpose purpose)
        => purpose switch
        {
            OtpPurpose.EmailVerification or
            OtpPurpose.PasswordReset => user.Email?.Value,
            _ => null
        };

    private static string BuildHtmlContent(string otp, OtpPurpose purpose)
    {
        var code = WebUtility.HtmlEncode(otp);

        var purposeMessage = purpose == OtpPurpose.PasswordReset
            ? "Use this code to reset your password."
            : "Use this code to verify your email address.";

        return $"""
            <!DOCTYPE html>
            <html>
              <body>
                <h1>FixNow</h1>
                <p>Your FixNow verification code is:</p>
                <h2 style="letter-spacing: 4px;">{code}</h2>
                <p>{purposeMessage}</p>
                <p>This code expires in {ExpirationMinutes} minutes.</p>
                <p>If you did not request this code, you can safely ignore this email.</p>
              </body>
            </html>
            """;
    }
}
