namespace Rest.Http
{
    internal class HttpRequest
    {
        public HttpVerb Method { get; private set; }
        public string Path { get; private set; }
        public string Version { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        public string Content { get; private set; }

        public HttpRequest(StreamReader reader)
        {
            Headers = new Dictionary<string, string>();

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
        }

        private void ParseMethod(string line)
        {
            string[] parts = line.Split(' ');
            Method = HttpVerbHelper.FromName(parts[0]);
            Path = parts[1];
            Version = parts[2];
        }

        private void ParseHeader(string line)
        {
            int splitIndex = line.IndexOf(':');
            string key = line.Substring(0, splitIndex);
            string value = line.Substring(splitIndex + 2);
            Headers.Add(key, value);
        }

        private void ParseContent(StreamReader reader)
        {
            string? contentLengthStr;
            if (Headers.TryGetValue("Content-Length", out contentLengthStr) && contentLengthStr != null)
            {
                int contentLength = int.Parse(contentLengthStr);

                char[] buffer = new char[contentLength];
                reader.Read(buffer, 0, contentLength);

                Content = new string(buffer);
            }
        }

        private enum Section
        {
            METHOD, HEADER, CONTENT
        }
    }
}
