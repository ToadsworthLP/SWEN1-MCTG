using System.Net;

namespace Rest.Http
{
    internal class HttpRequest : IApiRequest
    {
        public Method Method { get; private set; }
        public string Path { get; private set; }
        public string Content { get; private set; }
        public string? AuthToken { get; private set; }
        public string Version { get; private set; }
        public Dictionary<string, string> Headers { get; }
        public Dictionary<string, string> Parameters { get; }

        public HttpRequest(StreamReader reader)
        {
            Headers = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();

            string? line;
            Section currentSection = Section.METHOD;

            while (currentSection != Section.CONTENT)
            {
                line = reader.ReadLine();
                if (line == null) break;

                switch (currentSection)
                {
                    case Section.METHOD:
                        ParseMethod(line);
                        currentSection = Section.HEADER;
                        break;

                    case Section.HEADER:
                        if (line.Length == 0)
                        {
                            currentSection = Section.CONTENT;
                        }
                        else
                        {
                            ParseHeader(line);
                        }
                        break;
                }
            }

            ParseContent(reader);

            ParseGetParameters();

            if (Path.EndsWith('/')) Path = Path.Substring(0, Path.Length - 1);

            if (Headers.TryGetValue("Authorization", out string authInfo))
            {
                int splitIndex = Headers["Authorization"].IndexOf(' ');
                AuthToken = authInfo.Substring(splitIndex + 1);
            }
            else
            {
                AuthToken = null;
            }
        }

        private void ParseMethod(string line)
        {
            string[] parts = line.Split(' ');
            Method = HttpMethodHelper.FromName(parts[0]);
            Path = parts[1];
            Version = parts[2];
        }

        private void ParseHeader(string line)
        {
            int splitIndex = line.IndexOf(':');

            if (splitIndex + 2 >= line.Length) return;

            string key = line.Substring(0, splitIndex);
            string value = line.Substring(splitIndex + 2);
            Headers.Add(key, value);
        }

        private void ParseContent(StreamReader reader)
        {
            if (Headers.TryGetValue("Content-Length", out string? contentLengthStr) && contentLengthStr != null)
            {
                int contentLength = int.Parse(contentLengthStr);

                char[] buffer = new char[contentLength];
                reader.Read(buffer, 0, contentLength);

                Content = new string(buffer);
            }

            if (Content == null) Content = "{}";
        }

        private void ParseGetParameters()
        {
            int startIndex = Path.IndexOf('?');

            if (startIndex >= 0)
            {
                string parameters = Path.Substring(startIndex + 1);
                string[] parts = parameters.Split('&');

                foreach (string part in parts)
                {
                    int splitIndex = part.IndexOf('=');

                    if (splitIndex >= 0)
                    {
                        string key = WebUtility.UrlDecode(part.Substring(0, splitIndex));
                        string value = WebUtility.UrlDecode(part.Substring(splitIndex + 1));

                        Parameters.Add(key, value);
                    }
                    else
                    {
                        string key = WebUtility.UrlDecode(part);
                        Parameters.Add(key, "");
                    }
                }

                Path = Path.Substring(0, startIndex);
            }
        }

        private enum Section
        {
            METHOD, HEADER, CONTENT
        }
    }
}
