namespace Rest.ResponseTypes
{
    public class Unauthorized : IApiResponse
    {
        public string Status => "401 Unauthorized";

        public object? Content { get; }

        public Unauthorized() { }

        public Unauthorized(object content)
        {
            Content = content;
        }
    }
}
