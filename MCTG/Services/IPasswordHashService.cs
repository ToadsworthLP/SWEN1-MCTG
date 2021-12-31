namespace MCTG.Services
{
    internal interface IPasswordHashService
    {
        byte[] Hash(string input);
        bool Verify(byte[] hash, string input);
    }
}