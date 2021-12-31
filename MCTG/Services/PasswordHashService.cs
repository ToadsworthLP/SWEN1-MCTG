using System.Security.Cryptography;
using System.Text;

namespace MCTG.Services
{
    internal class PasswordHashService : IPasswordHashService
    {
        public byte[] Hash(string input)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.Default.GetBytes(input));
            }

            return hash;
        }

        public bool Verify(byte[] hash, string input)
        {
            byte[] inputHash = Hash(input);
            return inputHash == hash;
        }
    }
}
