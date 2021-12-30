using Rest;

namespace MCTG.Auth
{
    internal class AuthProvider : IAuthProvider
    {
        public Role currentRole;

        public bool IsAuthorized(object? expected, string? token)
        {
            Role role;
            if (token == "admin")
            {
                role = Role.ADMIN;
            }
            else if (token == "user")
            {
                role = Role.USER;
            }
            else
            {
                return false;
            }

            currentRole = role;

            return expected is Role && (Role)expected == role;
        }
    }
}
