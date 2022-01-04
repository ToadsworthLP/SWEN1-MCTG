namespace MCTG.Responses
{
    public class ErrorResponse
    {
        public string Error;

        public ErrorResponse(string error)
        {
            Error = error;
        }
    }
}
