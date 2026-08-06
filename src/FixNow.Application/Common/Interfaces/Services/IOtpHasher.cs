public interface IOtpHasher
{
    string Hash(string otp);

    bool Verify(string otp, string codeHash);
}
