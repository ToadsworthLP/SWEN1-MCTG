using Newtonsoft.Json;
using Rest.Http;
using Rest.ResponseTypes;

namespace Rest
{
    internal class RequestHandler
    {
        private readonly ControllerRegistry registry;
        private readonly StreamWriter writer;

        public RequestHandler(ControllerRegistry registry, StreamWriter writer)
        {
            this.registry = registry;
            this.writer = writer;
        }

        public void Handle(IApiRequest request)
        {
            var handlerInfo = registry.GetHandler(request);

            IApiResponse response;
            if (handlerInfo != null)
            {
                if (handlerInfo.ParameterType != null)
                {
                    try
                    {
                        object? parameter = JsonConvert.DeserializeObject(request.Content, handlerInfo.ParameterType);
                        response = handlerInfo.Handler.Invoke(parameter);
                    }
                    catch (JsonReaderException)
                    {
                        response = new BadRequest();
                    }
                }
                else
                {
                    response = handlerInfo.Handler.Invoke(null);
                }
            }
            else
            {
                response = new NotFound();
            }

            HttpResponse httpResponse = new HttpResponse(response);
            httpResponse.Send(writer);

            writer.Flush();
            writer.Close();
        }
    }
}
