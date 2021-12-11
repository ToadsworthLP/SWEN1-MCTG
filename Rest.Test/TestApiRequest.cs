using System.Collections.Generic;

namespace Rest.Test
{
    internal class TestApiRequest : IApiRequest
    {
        public Method Method { get; set; }

        public string Path { get; set; }

        public string? AuthToken { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public string Content { get; set; }
    }
}
