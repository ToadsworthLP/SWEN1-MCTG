namespace Rest.Test.Intragration
{
    public class TestService : ITestService
    {
        private int i = 0;

        public string GetTheThing()
        {
            return $"Calls to same instance: {i++}";
        }
    }
}
