namespace Rest.ResponseTypes
{
    internal class InternalServerError : IApiResponse
    {
        public string Status => "500 Internal Server Error";

        public object? Content { get; }

        public InternalServerError() { }

        public InternalServerError(object content)
        {
            Content = content;
        }
    }
}
