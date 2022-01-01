using MCTG.Auth;
using MCTG.Config;
using MCTG.Controllers;
using MCTG.Services;
using Rest;

namespace MCTG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RestServer server = new RestServer(System.Net.IPAddress.Any, 10001);

            server.RequestFinished += (sender, args) => AppDbContext.NotifyEndOfRequest();

            server.AddController<UserController>();
            server.AddController<SessionController>();

            server.AddScoped<IPasswordHashService, PasswordHashService>();
            server.AddScoped<ITokenService, TokenService>();

            server.AddScoped<AppDbContext>();

            server.AddAuth<AuthProvider>();

            server.Start();
        }
    }
}