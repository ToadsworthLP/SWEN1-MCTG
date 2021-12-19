using Ninject;
using Ninject.Extensions.NamedScope;
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

        private IKernel kernel;

        public RestServer(IPAddress adress, int port)
        {
            this.address = adress;
            this.port = port;

            controllers = new ControllerRegistry();
            kernel = new StandardKernel();
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
                        try
                        {
                            request = new HttpRequest(reader);
                        }
                        catch
                        {
                            return; // Ignore requests that don't follow the HTTP standard
                        }

                        RequestHandler requestHandler;

                        if (authHandlerType == null)
                        {
                            requestHandler = new RequestHandler(controllers, writer, kernel);
                        }
                        else
                        {
                            IAuthProvider? authHandler = (IAuthProvider)Activator.CreateInstance(authHandlerType);
                            requestHandler = new RequestHandler(controllers, writer, authHandler, kernel);
                        }

                        requestHandler.Handle(request);
                    }
                });
            }
        }

        public void AddController<T>()
        {
            controllers.AddController<T>();
        }

        public void AddAuth<T>() where T : IAuthProvider, new()
        {
            authHandlerType = typeof(T);
        }

        public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            kernel.Bind<TInterface>().To<TImplementation>().InSingletonScope();
        }

        public void AddSingleton<TImplementation>()
        {
            kernel.Bind<TImplementation>().ToSelf().InSingletonScope();
        }

        public void AddScoped<TInterface, TImplementation>() where TImplementation : TInterface
        {
            kernel.Bind<TInterface>().To<TImplementation>().InCallScope();
        }

        public void AddScoped<TImplementation>()
        {
            kernel.Bind<TImplementation>().ToSelf().InCallScope();
        }

        public void AddTransient<TInterface, TImplementation>() where TImplementation : TInterface
        {
            kernel.Bind<TInterface>().To<TImplementation>().InTransientScope();
        }

        public void AddTransient<TImplementation>()
        {
            kernel.Bind<TImplementation>().ToSelf().InTransientScope();
        }
    }
}