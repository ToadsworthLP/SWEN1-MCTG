namespace Rest
{
    public interface IApiRequest
    {
        public Method Method { get; }
        public string Path { get; }
        public string? AuthToken { get; }
        public Dictionary<string, string> Parameters { get; }
        public string Content { get; }
    }
}
