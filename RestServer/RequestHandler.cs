using Newtonsoft.Json;
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
                object parameter = JsonConvert.DeserializeObject(request.Content, handlerInfo.ParameterType); // TODO handle JSON problem as invalid request
                response = handlerInfo.Handler.Invoke(parameter);
            }
            else
            {
                response = new NotFound();
            }

            string responseJson = JsonConvert.SerializeObject(response.Content);

            WriteLine(writer, $"HTTP/1.1 {response.Status}");
            WriteLine(writer, "Server: Test Server");
            WriteLine(writer, $"Current Time: {DateTime.Now}");
            WriteLine(writer, $"Content-Length: {responseJson.Length}");
            WriteLine(writer, "Content-Type: application/json; charset=utf-8");
            WriteLine(writer, "");
            WriteLine(writer, responseJson);

            writer.WriteLine();
            writer.Flush();
            writer.Close();
        }

        private void WriteLine(StreamWriter writer, string s)
        {
            writer.WriteLine(s);
        }
    }
}
