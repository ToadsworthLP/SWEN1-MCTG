namespace Rest.Http
{
    internal class HttpVerbHelper
    {
        private static readonly Dictionary<string, HttpVerb> Verbs = new();

        static HttpVerbHelper()
        {
            foreach (HttpVerb verb in Enum.GetValues(typeof(HttpVerb)))
            {
                Verbs.Add(verb.ToString(), verb);
            }
        }

        public static HttpVerb FromName(string name)
        {
            return Verbs[name];
        }

        public static string ToName(HttpVerb verb)
        {
            return verb.ToString();
        }
    }
}
