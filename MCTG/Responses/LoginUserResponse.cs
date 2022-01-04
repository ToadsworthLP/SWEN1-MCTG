namespace MCTG.Responses
{
    public class LoginUserResponse
    {
        public string Token;

        public LoginUserResponse(string token)
        {
            Token = token;
        }
    }
}
