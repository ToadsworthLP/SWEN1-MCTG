namespace Rest.ResponseTypes
{
    public class NotFound : IApiResponse
    {
        public string Status => "404 Not Found";

        public object? Content { get; }

        public NotFound() { }

        public NotFound(object content)
        {
            Content = content;
        }
    }
}
