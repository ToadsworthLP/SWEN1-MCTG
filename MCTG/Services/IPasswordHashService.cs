namespace MCTG.Services
{
    public interface IPasswordHashService
    {
        byte[] Hash(string input);
        bool Verify(byte[] hash, string input);
    }
}