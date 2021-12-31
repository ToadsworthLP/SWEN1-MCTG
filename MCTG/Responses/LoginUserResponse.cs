namespace MCTG.Responses
{
    internal class LoginUserResponse
    {
        public string Token;

        public LoginUserResponse(string token)
        {
            Token = token;
        }
    }
}
