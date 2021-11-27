namespace Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class MethodAttribute : Attribute
    {
        public Method Method { get; }

        public MethodAttribute(Method method)
        {
            this.Method = method;
        }
    }
}
