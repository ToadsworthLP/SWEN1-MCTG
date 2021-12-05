using MCTG.Auth;
using MCTG.Controllers;
using MCTG.Services;
using Rest;

namespace MCTG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RestServer server = new RestServer(System.Net.IPAddress.Any, 25567);

            server.AddController<EchoController>();

            server.AddScoped<ITestService, TestService>();

            server.AddAuth<AuthProvider>();
            server.Start();
        }
    }
}