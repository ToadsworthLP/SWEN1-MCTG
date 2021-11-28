using Rest.Http;
using System.Net;
using System.Net.Sockets;

namespace Rest
{
    public class RestServer
    {
        private IPAddress address;
        private int port;

        private ControllerRegistry controllers;

        private Type authHandlerType;

        public RestServer(IPAddress adress, int port)
        {
            this.address = adress;
            this.port = port;

            controllers = new ControllerRegistry();
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(address, port);
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
                        RequestHandler requestHandler;

                        if (authHandlerType == null)
                        {
                            requestHandler = new RequestHandler(controllers, writer);
                        }
                        else
                        {
                            IAuthHandler? authHandler = (IAuthHandler)Activator.CreateInstance(authHandlerType);
                            requestHandler = new RequestHandler(controllers, writer, authHandler);
                        }

                        requestHandler.Handle(request);
                    }
                });
            }
        }

        public void AddController<T>() where T : new()
        {
            controllers.AddController<T>();
        }

        public void AddAuth<T>() where T : IAuthHandler, new()
        {
            authHandlerType = typeof(T);
        }
    }
}