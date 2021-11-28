namespace Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestrictAttribute : Attribute
    {
        public object? Restriction { get; }

        public RestrictAttribute(object? restriction)
        {
            Restriction = restriction;
        }
    }
}
