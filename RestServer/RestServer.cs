using Rest.Http;
using System.Net;
using System.Net.Sockets;

namespace Rest
{
    public class RestServer
    {
        private IPAddress Address;
        private int Port;

        public RestServer(IPAddress adress, int port)
        {
            this.Address = adress;
            this.Port = port;
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(Address, Port);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() =>
                {
                    using (StreamReader reader = new StreamReader(client.GetStream()))
                    using (StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true })
                    {
                        HttpRequest request;
                        request = new HttpRequest(reader);

                        RequestHandler requestHandler = new RequestHandler(writer);
                        requestHandler.Handle(request);
                    }
                });
            }
        }
    }
}