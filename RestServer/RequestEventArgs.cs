namespace Rest
{
    public class RequestEventArgs : EventArgs
    {
        public IApiRequest Request;

        public RequestEventArgs(IApiRequest request)
        {
            Request = request;
        }
    }
}
