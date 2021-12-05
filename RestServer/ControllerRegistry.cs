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

                    if (methodAttr == null) continue;

                    if (!method.ReturnType.IsAssignableTo(typeof(IApiResponse)))
                    {
                        throw new ArgumentException($"Invalid return type in method {methodAttr.Method} in controller {typeof(T).Name}. Handler methods must return a type implementing IApiResponse.");
                    }

                    bool hasBody = false, hasRouteParam = false;

                    ParameterInfo[] parameters = method.GetParameters();

                    foreach (ParameterInfo parameter in parameters)
                    {
                        ControllerMethodParameterAttribute? controllerMethodParameterAttribute = parameter.GetCustomAttribute<ControllerMethodParameterAttribute>();
                        if (controllerMethodParameterAttribute != null)
                        {
                            if (controllerMethodParameterAttribute is FromBodyAttribute)
                            {
                                hasBody = true;
                            }
                            else if (controllerMethodParameterAttribute is FromRouteAttribute)
                            {
                                hasRouteParam = true;
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid or missing attribute at parameter {parameter.Name} of method {methodAttr.Method} in controller {typeof(T).Name}.");
                        }
                    }

                    RouteInfo route = new RouteInfo(typeof(T), methodAttr.Method, hasBody, hasRouteParam);

                    if (Routes.ContainsKey(route))
                    {
                        throw new ArgumentException($"Multiple handlers with similar signatures found for method {methodAttr.Method} in controller {typeof(T).Name}.");
                    }

                    Routes.Add(route, method);
                }
            }
            else
            {
                throw new ArgumentException($"Passed controller type {typeof(T).Name} does not have a Route attribute.");
            }
        }

        public HandlerInfo? GetHandler(IApiRequest request)
        {
            bool hasRouteParam = false;

            bool hasBody = request.Content != "";

            Type? controllerType;
            if (!Controllers.TryGetValue(request.Path, out controllerType))
            {
                int lastPartStartIndex = request.Path.LastIndexOf('/');
                if (lastPartStartIndex >= 0)
                {
                    string pathWithoutLastPart = request.Path.Substring(0, lastPartStartIndex);
                    if (!Controllers.TryGetValue(pathWithoutLastPart, out controllerType))
                    {
                        return null;
                    }

                    hasRouteParam = true;
                }
            }

            MethodInfo methodInfo;
            if (!Routes.TryGetValue(new RouteInfo(controllerType, request.Method, hasBody, hasRouteParam), out methodInfo))
            {
                return null;
            }

            return new HandlerInfo(controllerType, methodInfo);
        }
    }

    internal record RouteInfo(Type ControllerType, Method Method, bool HasBody, bool HasRouteParam);

    public record HandlerInfo(Type ControllerType, MethodInfo Handler);
}
