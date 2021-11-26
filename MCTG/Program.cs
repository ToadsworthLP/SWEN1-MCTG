using Rest;

namespace MCTG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RestServer server = new RestServer(System.Net.IPAddress.Any, 25567);
            server.Start();
        }
    }
}