using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Storage;

namespace FixNow.Application.Features.ServiceRequests.Commands.UploadServiceRequestImage;

public sealed class UploadServiceRequestImageCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IFileStorage fileStorage,
    ICurrentUser currentUser)
    : ICommandHandler<
        UploadServiceRequestImageCommand,
        Result<UploadServiceRequestImageResponse>>
{
    public async Task<Result<UploadServiceRequestImageResponse>> Handle(
        UploadServiceRequestImageCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's customer profile (ownership is derived
        //    from the authenticated user, never from the client).
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the service request.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            command.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. Verify the service request belongs to the current customer.
        if (serviceRequest.CustomerProfileId != customerProfile.Id)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. The storage location is derived server-side from the owner and
        //    the request - never from client-supplied paths.
        var key = BuildImageKey(
            customerProfile.Id,
            serviceRequest.Id,
            command.FileName);

        // 5. Upload the image to Cloudinary.
        var storeResult = await fileStorage.StoreAsync(
            key,
            command.Content,
            command.ContentType,
            cancellationToken);

        if (storeResult.IsError)
        {
            return storeResult.Errors;
        }

        // 6. Create the image entity using the Cloudinary reference.
        var imageResult = ServiceRequestImage.Create(
            id: Guid.NewGuid(),
            serviceRequestId: serviceRequest.Id,
            imageKey: storeResult.Value);

        if (imageResult.IsError)
        {
            await fileStorage.DeleteAsync(
                storeResult.Value,
                cancellationToken);

            return imageResult.Errors;
        }

        // 7. Attach the image to the request.
        var addResult = serviceRequest.AddImage(imageResult.Value);

        if (addResult.IsError)
        {
            await fileStorage.DeleteAsync(
                storeResult.Value,
                cancellationToken);

            return addResult.Errors;
        }

        // 8. Track the new image so it is inserted.
        await serviceRequestRepository.AddImageAsync(
            imageResult.Value,
            cancellationToken);

        // 9. Persist the change (committed by the transaction pipeline).
        serviceRequestRepository.Update(serviceRequest);

        return new UploadServiceRequestImageResponse(
            ImageId: imageResult.Value.Id,
            ImageKey: imageResult.Value.ImageKey);
    }

    private static string BuildImageKey(
        Guid customerProfileId,
        Guid serviceRequestId,
        string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        var storedFileName = $"{Guid.NewGuid():N}{extension}";

        return $"{UploadServiceRequestImageCommand.ProblemImagesFolderPrefix}/{customerProfileId}/{serviceRequestId}/{storedFileName}";
    }
}
