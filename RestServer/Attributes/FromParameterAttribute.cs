namespace Rest.Attributes
{
    public class FromParameterAttribute : ControllerMethodParameterAttribute
    {
        public string Parameter { get; }

        public FromParameterAttribute(string parameter)
        {
            this.Parameter = parameter;
        }
    }
}
