using MCTG.Models;
using MCTG.Services;
using Rest;

namespace MCTG.Auth
{
    internal class AuthProvider : IAuthProvider
    {
        public static ThreadLocal<User?> CurrentUser = new ThreadLocal<User?>();

        private readonly ITokenService tokenService;

        public AuthProvider(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public bool IsAuthorized(object? expected, string? token)
        {
            if (expected is Role expectedRole)
            {
                if (token == null) return false;

                User? tokenUser = tokenService.ReadToken(token);

                if (tokenUser == null) return false;

                CurrentUser.Value = tokenUser;

                return tokenUser.Role >= expectedRole;
            }

            CurrentUser.Value = null;
            return false;
        }
    }
}
