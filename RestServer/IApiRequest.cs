namespace Rest
{
    public interface IApiRequest
    {
        public Method Method { get; }
        public string Path { get; }
        public string? AuthToken { get; }
        public string Content { get; }

    }
}
