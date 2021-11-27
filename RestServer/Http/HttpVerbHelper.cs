namespace Rest.Http
{
    internal class HttpVerbHelper
    {
        private static readonly Dictionary<string, Method> Verbs = new();

        static HttpVerbHelper()
        {
            foreach (Method verb in Enum.GetValues(typeof(Method)))
            {
                Verbs.Add(verb.ToString(), verb);
            }
        }

        public static Method FromName(string name)
        {
            return Verbs[name];
        }

        public static string ToName(Method verb)
        {
            return verb.ToString();
        }
    }
}
