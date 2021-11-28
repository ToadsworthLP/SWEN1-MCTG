namespace Rest.Http
{
    internal class HttpMethodHelper
    {
        private static readonly Dictionary<string, Method> Methods = new();

        static HttpMethodHelper()
        {
            foreach (Method verb in Enum.GetValues(typeof(Method)))
            {
                Methods.Add(verb.ToString(), verb);
            }
        }

        public static Method FromName(string name)
        {
            return Methods[name];
        }

        public static string ToName(Method verb)
        {
            return verb.ToString();
        }
    }
}
