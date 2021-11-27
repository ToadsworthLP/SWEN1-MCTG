using Rest.Http;
using System.Net;
using System.Net.Sockets;

namespace Rest
{
    public class RestServer
    {
        private IPAddress Address;
        private int Port;

        private ControllerRegistry controllers;

        public RestServer(IPAddress adress, int port)
        {
            this.Address = adress;
            this.Port = port;

            controllers = new ControllerRegistry();
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
                        IApiRequest request;
                        request = new HttpRequest(reader);

                        RequestHandler requestHandler = new RequestHandler(controllers, writer);
                        requestHandler.Handle(request);
                    }
                });
            }
        }

        public void AddController<T>() where T : new()
        {
            controllers.AddController<T>();
        }
    }
}