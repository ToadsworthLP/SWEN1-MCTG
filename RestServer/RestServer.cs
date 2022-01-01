using Ninject;
using Ninject.Extensions.NamedScope;
using Rest.Http;
using System.Net;
using System.Net.Sockets;

namespace Rest
{
    public class RestServer
    {
        public event EventHandler<RequestEventArgs> RequestStarted;
        public event EventHandler<RequestEventArgs> RequestFinished;

        private IPAddress address;
        private int port;

        private ControllerRegistry controllers;
        private IKernel kernel;

        private bool useAuth;

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

                        if (RequestStarted != null) RequestStarted(this, new RequestEventArgs(request));

                        RequestHandler requestHandler;

                        if (useAuth)
                        {
                            IAuthProvider? authHandler = kernel.Get<IAuthProvider>();
                            requestHandler = new RequestHandler(controllers, writer, authHandler, kernel);
                        }
                        else
                        {
                            requestHandler = new RequestHandler(controllers, writer, kernel);
                        }

                        requestHandler.Handle(request);

                        if (RequestFinished != null) RequestFinished(this, new RequestEventArgs(request));
                    }

                });
            }
        }

        public void AddController<T>()
        {
            controllers.AddController<T>();
        }

        public void AddAuth<T>() where T : IAuthProvider
        {
            useAuth = true;
            AddScoped<IAuthProvider, T>();
        }

        public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            kernel.Bind<TInterface>().To<TImplementation>().InSingletonScope();
        }

        public void AddSingleton<TImplementation>()
        {
            kernel.Bind<TImplementation>().ToSelf().InSingletonScope();
        }

        public void AddSingletonInstance<T>(T instance)
        {
            kernel.Bind<T>().ToConstant(instance);
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