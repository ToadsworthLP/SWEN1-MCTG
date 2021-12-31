using MCTG.Models;

namespace MCTG.Services
{
    internal interface ITokenService
    {
        public string GenerateToken(User user);
        public User? ReadToken(string token);
        public void InvalidateToken(string token);
    }
}
