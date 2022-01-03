namespace Rest.ResponseTypes
{
    public class Accepted : IApiResponse
    {
        public string Status => "202 Accepted";

        public object? Content { get; }

        public Accepted() { }

        public Accepted(object? content)
        {
            Content = content;
        }
    }
}
