namespace MCTG.Services
{
    public class TestService : ITestService
    {
        private int i = 0;

        public string GetTheThing()
        {
            return $"Calls: {i++}";
        }
    }
}
