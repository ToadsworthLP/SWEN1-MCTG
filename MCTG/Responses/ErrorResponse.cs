namespace MCTG.Responses
{
    internal class ErrorResponse
    {
        public string Error;

        public ErrorResponse(string error)
        {
            Error = error;
        }
    }
}
