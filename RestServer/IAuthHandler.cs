namespace Rest
{
    public interface IAuthHandler
    {
        public bool IsAuthorized(object? expected, string? token);
    }
}
