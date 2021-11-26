using Rest.Http;

namespace Rest
{
    internal class RequestHandler
    {
        private readonly StreamWriter writer;

        public RequestHandler(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void Handle(HttpRequest request)
        {
            WriteLine(writer, "HTTP/1.1 200 OK");
            WriteLine(writer, "Server: Test Server");
            WriteLine(writer, $"Current Time: {DateTime.Now}");
            WriteLine(writer, $"Content-Length: {request.Content.Length}");
            WriteLine(writer, "Content-Type: application/json; charset=utf-8");
            WriteLine(writer, "");
            WriteLine(writer, request.Content);

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
