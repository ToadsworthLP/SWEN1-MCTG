namespace Rest.ResponseTypes
{
    public class BadRequest : IApiResponse
    {
        public string Status => "400 Bad Request";

        public object? Content { get; }

        public BadRequest() { }

        public BadRequest(object content)
        {
            Content = content;
        }
    }
}