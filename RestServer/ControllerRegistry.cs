using Rest.Attributes;
using System.Reflection;

namespace Rest
{
    public class ControllerRegistry
    {
        private Dictionary<string, Type> Controllers { get; }
        private Dictionary<(Type, Method), MethodInfo> Routes { get; }

        public ControllerRegistry()
        {
            Controllers = new Dictionary<string, Type>();
            Routes = new Dictionary<(Type, Method), MethodInfo>();
        }

        public void AddController<T>() where T : new()
        {
            Attribute? routeAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(RouteAttribute));
            if (routeAttribute != null)
            {
                Controllers.Add(((RouteAttribute)routeAttribute).Route, typeof(T));

                Type[] interfaces = typeof(T).FindInterfaces((Type typeObj, object criteriaOb) =>
                {
                    return typeObj.IsAssignableTo(typeof(IControllerMethod)) && typeObj != typeof(IControllerMethod);
                }, null);

                foreach (Type type in interfaces)
                {
                    foreach (MethodInfo method in type.GetMethods())
                    {
                        MethodAttribute? methodAttr = method.GetCustomAttribute<MethodAttribute>();
                        if (methodAttr == null) continue;

                        if (!Routes.ContainsKey((typeof(T), methodAttr.Method)))
                        {
                            Routes.Add((typeof(T), methodAttr.Method), method);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Passed controller type does not have a Route attribute.");
            }
        }

        public HandlerInfo? GetHandler(IApiRequest request)
        {
            Type controllerType;
            if (!Controllers.TryGetValue(request.Path, out controllerType))
            {
                return null;
            }

            MethodInfo methodInfo;
            if (!Routes.TryGetValue((controllerType, request.Method), out methodInfo))
            {
                return null;
            }

            object controller = Activator.CreateInstance(controllerType);
            return new HandlerInfo((parameter) =>
            {
                return (IApiResponse)methodInfo.Invoke(controller, new object[1] { parameter });
            }, methodInfo.DeclaringType.GenericTypeArguments[0]);
        }
    }

    public record HandlerInfo(Func<object, IApiResponse> Handler, Type ParameterType);
}
