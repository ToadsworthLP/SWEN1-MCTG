using MCTG.Models;
using MCTG.Services;
using Rest;

namespace MCTG.Auth
{
    internal class AuthProvider : IAuthProvider
    {
        public static User? CurrentUser { get { return currentUser.Value; } }

        private static ThreadLocal<User?> currentUser = new ThreadLocal<User?>();

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

                currentUser.Value = tokenUser;

                return tokenUser.Role >= expectedRole;
            }

            currentUser.Value = null;
            return false;
        }
    }
}
