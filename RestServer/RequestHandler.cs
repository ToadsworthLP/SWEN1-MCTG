using Newtonsoft.Json;
using Rest.Attributes;
using Rest.Http;
using Rest.ResponseTypes;
using System.Reflection;

namespace Rest
{
    internal class RequestHandler
    {
        private readonly ControllerRegistry registry;
        private readonly StreamWriter writer;
        private readonly IAuthHandler? authHandler;

        public RequestHandler(ControllerRegistry registry, StreamWriter writer)
        {
            this.registry = registry;
            this.writer = writer;
            this.authHandler = null;
        }

        public RequestHandler(ControllerRegistry registry, StreamWriter writer, IAuthHandler? authHandler)
        {
            this.registry = registry;
            this.writer = writer;
            this.authHandler = authHandler;
        }

        public void Handle(IApiRequest request)
        {
            IApiResponse? response = null;

            Type? controllerType = registry.GetControllerType(request);
            MethodInfo? methodInfo = registry.GetHandler(request);

            if (ValidateRequest(request, ref response, methodInfo))
            {
                object? controller = Activator.CreateInstance(controllerType);

                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                Type? parameterType = parameterInfos.Length > 0 ? parameterInfos[0].ParameterType : null;

                if (parameterType != null)
                {
                    try
                    {
                        object? parameter = JsonConvert.DeserializeObject(request.Content, parameterType);
                        response = (IApiResponse)methodInfo.Invoke(controller, new object[1] { parameter });
                    }
                    catch (JsonReaderException)
                    {
                        response = new BadRequest();
                    }
                }
                else
                {
                    response = (IApiResponse)methodInfo.Invoke(controller, null);
                }
            }

            HttpResponse httpResponse = new HttpResponse(response);
            httpResponse.Send(writer);

            writer.Flush();
            writer.Close();
        }

        private bool ValidateRequest(IApiRequest request, ref IApiResponse? response, MethodInfo? handler)
        {
            if (handler == null)
            {
                response = new NotFound();
                return false;
            }

            RestrictAttribute? restrictAttr = handler.GetCustomAttribute<RestrictAttribute>();
            if (restrictAttr != null && authHandler != null && (!authHandler.IsAuthorized(restrictAttr.Restriction, request.AuthToken)))
            {
                response = new Unauthorized();
                return false;
            }

            return true;
        }
    }
}
