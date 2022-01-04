using MCTG.Models;

namespace MCTG.Services
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
        public User? ReadToken(string token);
        public void InvalidateToken(string token);
    }
}
