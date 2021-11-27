namespace Rest
{
    public interface IApiResponse
    {
        public string Status { get; }
        public object? Content { get; }
    }
}
