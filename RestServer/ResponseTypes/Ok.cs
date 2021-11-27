namespace Rest.ResponseTypes
{
    public class Ok : IApiResponse
    {
        public string Status => "200 OK";

        public object? Content { get; }

        public Ok() { }
        public Ok(object content)
        {
            Content = content;
        }
    }
}
