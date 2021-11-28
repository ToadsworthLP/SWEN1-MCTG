using Rest.Attributes;
using System.Reflection;

namespace Rest
{
    public class ControllerRegistry
    {
        private Dictionary<string, Type> Controllers { get; }
        private Dictionary<RouteInfo, MethodInfo> Routes { get; }

        public ControllerRegistry()
        {
            Controllers = new Dictionary<string, Type>();
            Routes = new Dictionary<RouteInfo, MethodInfo>();
        }

        public void AddController<T>() where T : new()
        {
            Attribute? routeAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(RouteAttribute));
            if (routeAttribute != null)
            {
                Controllers.Add(((RouteAttribute)routeAttribute).Route, typeof(T));

                MethodInfo[] candidates = typeof(T).GetMethods();
                foreach (MethodInfo method in candidates)
                {
                    MethodAttribute? methodAttr = method.GetCustomAttribute<MethodAttribute>();
                    bool hasParameter = method.GetParameters().Length > 0;

                    if (methodAttr == null) continue;

                    if (Routes.ContainsKey(new RouteInfo(typeof(T), methodAttr.Method, hasParameter)))
                    {
                        throw new ArgumentException($"Multiple handlers with equal argument count found for method {methodAttr.Method} in controller {typeof(T).Name}.");
                    }

                    if (!method.ReturnType.IsAssignableTo(typeof(IApiResponse)))
                    {
                        throw new ArgumentException($"Invalid return type in method {methodAttr.Method} in controller {typeof(T).Name}. Handler methods must return a type implementing IApiResponse.");
                    }

                    if (method.GetParameters().Length > 1)
                    {
                        throw new ArgumentException($"Invalid parameters in method {methodAttr.Method} in controller {typeof(T).Name}. Handler methods must take at most one parameter.");
                    }

                    Routes.Add(new RouteInfo(typeof(T), methodAttr.Method, hasParameter), method);
                }
            }
            else
            {
                throw new ArgumentException($"Passed controller type {typeof(T).Name} does not have a Route attribute.");
            }
        }

        public Type? GetControllerType(IApiRequest request)
        {
            Type controllerType;
            if (!Controllers.TryGetValue(request.Path, out controllerType))
            {
                return null;
            }

            return controllerType;
        }

        public MethodInfo? GetHandler(IApiRequest request)
        {
            Type controllerType;
            if (!Controllers.TryGetValue(request.Path, out controllerType))
            {
                return null;
            }

            MethodInfo methodInfo;
            if (!Routes.TryGetValue(new RouteInfo(controllerType, request.Method, request.Content != ""), out methodInfo))
            {
                return null;
            }

            return methodInfo;
        }
    }

    public record HandlerInfo(Func<object?, IApiResponse?> Handler, Type? ParameterType);

    internal record RouteInfo(Type ControllerType, Method Method, bool HasParameter);
}
