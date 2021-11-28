using Newtonsoft.Json;

namespace Rest.Http
{
    internal class HttpResponse
    {
        private const string SERVER = "HTTP REST Server";
        private readonly string httpResponse;

        public HttpResponse(IApiResponse response)
        {
            string responseJson = JsonConvert.SerializeObject(response.Content);
            if (responseJson == "null")
            {
                httpResponse = $"HTTP/1.1 {response.Status}\nServer: {SERVER}\nCurrent Time: {DateTime.Now}\nContent-Length: {0}\nContent-Type: application/json; charset=utf-8";
            }
            else
            {
                httpResponse = $"HTTP/1.1 {response.Status}\nServer: {SERVER}\nCurrent Time: {DateTime.Now}\nContent-Length: {responseJson.Length}\nContent-Type: application/json; charset=utf-8\n\n{responseJson}";
            }

        }

        public void Send(StreamWriter writer)
        {
            writer.Write(httpResponse);
        }
    }
}
