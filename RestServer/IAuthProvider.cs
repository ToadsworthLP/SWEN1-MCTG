namespace Rest
{
    public interface IAuthProvider
    {
        public bool IsAuthorized(object? expected, string? token);
    }
}
