using Newtonsoft.Json;
using Ninject;
using Rest.Attributes;
using Rest.Http;
using Rest.ResponseTypes;
using System.Net;
using System.Reflection;

namespace Rest
{
    internal class RequestHandler
    {
        private readonly ControllerRegistry registry;
        private readonly StreamWriter writer;
        private readonly IAuthProvider? authHandler;
        private readonly IKernel kernel;

        public RequestHandler(ControllerRegistry registry, StreamWriter writer, IKernel kernel)
        {
            this.registry = registry;
            this.writer = writer;
            this.authHandler = null;
            this.kernel = kernel;
        }

        public RequestHandler(ControllerRegistry registry, StreamWriter writer, IAuthProvider? authHandler, IKernel kernel)
        {
            this.registry = registry;
            this.writer = writer;
            this.authHandler = authHandler;
            this.kernel = kernel;
        }

        public void Handle(IApiRequest request)
        {
            IApiResponse? response = null;

            HandlerInfo? handlerInfo = registry.GetHandler(request);

            if (ValidateRequest(request, ref response, handlerInfo))
            {
                ParameterInfo[] methodParameters = handlerInfo.Handler.GetParameters();
                object?[] parameterValues = new object[methodParameters.Length];

                try
                {
                    for (int i = 0; i < methodParameters.Length; i++)
                    {
                        ParameterInfo parameter = methodParameters[i];

                        ControllerMethodParameterAttribute? attribute = parameter.GetCustomAttribute<ControllerMethodParameterAttribute>();
                        if (attribute != null)
                        {
                            if (attribute is FromBodyAttribute)
                            {
                                parameterValues[i] = JsonConvert.DeserializeObject(request.Content, parameter.ParameterType);
                            }
                            else if (attribute is FromRouteAttribute)
                            {
                                int lastPartStartIndex = request.Path.LastIndexOf('/');
                                if (lastPartStartIndex >= 0)
                                {
                                    parameterValues[i] = WebUtility.UrlDecode(request.Path.Substring(lastPartStartIndex + 1));
                                }
                            }
                            else if (attribute is FromParameterAttribute)
                            {
                                FromParameterAttribute fromParameterAttribute = (FromParameterAttribute)attribute;
                                string value;
                                if (request.Parameters.TryGetValue(fromParameterAttribute.Parameter, out value))
                                {
                                    parameterValues[i] = value;
                                }
                            }
                        }
                        else
                        {
                            parameterValues[i] = null;
                        }
                    }

                    object? controller = kernel.Get(handlerInfo.ControllerType);

                    if (methodParameters.Length > 0)
                    {
                        response = (IApiResponse?)handlerInfo.Handler.Invoke(controller, parameterValues);
                    }
                    else
                    {
                        response = (IApiResponse?)handlerInfo.Handler.Invoke(controller, null);
                    }
                }
                catch (JsonException)
                {
                    response = new BadRequest();
                }
                catch
                {
                    response = new InternalServerError();
                }
            }

            HttpResponse httpResponse = new HttpResponse(response);
            httpResponse.Send(writer);

            writer.Flush();
        }

        private bool ValidateRequest(IApiRequest request, ref IApiResponse? response, HandlerInfo? handlerInfo)
        {
            if (handlerInfo == null)
            {
                response = new NotFound();
                return false;
            }

            RestrictAttribute? restrictAttr = handlerInfo.Handler.GetCustomAttribute<RestrictAttribute>();
            if (restrictAttr != null && authHandler != null && (!authHandler.IsAuthorized(restrictAttr.Restriction, request.AuthToken)))
            {
                response = new Unauthorized();
                return false;
            }

            return true;
        }
    }
}
