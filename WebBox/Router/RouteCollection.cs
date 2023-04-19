using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebBox.Controller;

namespace WebBox.Router;
public partial class RouteCollection
{
    private readonly WebBoxedServer _webBoxed;

    public RouteCollection(WebBoxedServer webBoxed)
    {
        _webBoxed = webBoxed;
    }

    private Dictionary<string, EndpointInfo> Endpoints
    {
        get;
    } = new();

    public void AddRoutes(ControllerDescriptor controller)
    {
        var builder = new EndpointsBuilder(Endpoints, controller.Path);
        controller.Instance.BuildRoutes(builder);
    }

    public RouteInfo? MapRoute(IEnumerable<ControllerDescriptor> controllers,string route)
    {
        var pattern = Endpoints.Keys.FirstOrDefault(p => CheckPathPattern(p, route));
        if (pattern == null)
        {
            return null;
        }
        
        var endpoint = Endpoints[pattern];
        var method = endpoint.Method;
        var controller = controllers.First(x => x.ControllerType.Equals(method.DeclaringType)).Instance;
        var parameters = method.GetParameters();

        // Подготовка параметров
        var pathParts = route.Split('/');
        var patternParts = pattern.Split('/');
        var regex = MyRegex();
        var matches = regex.Matches(pattern);
        var args = new List<object>();
        foreach (var parameter in parameters)
        {
            var paramName = parameter.Name;
            var patternParam = "{" + paramName + "}";
            var paramIndex = Array.IndexOf(patternParts, patternParam);
            var pathPart = pathParts[paramIndex];
            var match = matches.FirstOrDefault(m => m.Groups[1].Value == paramName);
            if (match != null)
            {
                var paramType = parameter.ParameterType;
                var paramValue = Convert.ChangeType(pathPart, paramType);
                args.Add(paramValue);
            }
            else
            {
                args.Add(null);
            }
        }
        
        return new RouteInfo(controller, () =>
        {
            var result = method.Invoke(controller, args.ToArray());
            return result;
        });
    }

    public static bool CheckPathPattern(string pattern, string path)
    {
        // Разделение паттерна и пути на части
        var patternParts = pattern.Split('/');
        var pathParts = path.Split('/');

        // Проверка количества частей паттерна и пути
        if (patternParts.Length != pathParts.Length)
        {
            return false;
        }

        // Создание регулярного выражения для каждого параметра
        var regex = MyRegex();
        var patternRegex = pattern.Replace(".", @"\.").Replace("/", @"\/");
        var matches = regex.Matches(pattern);
        foreach (Match match in matches)
        {
            var paramName = match.Groups[1].Value;
            var patternParam = "{" + paramName + "}";
            var paramIndex = Array.IndexOf(patternParts, patternParam);
            var patternPart = patternParts[paramIndex];
            var regexPart = "(?<" + paramName + ">.+)";
            patternRegex = patternRegex.Replace(patternPart, regexPart);
        }

        // Проверка соответствия паттерна и пути
        var pathRegex = new Regex("^" + patternRegex + "$");
        var pathMatch = pathRegex.Match(path);
        return pathMatch.Success;
    }

    class EndpointsBuilder : IEndpointsBuilder
    {
        private readonly string _prefix;
        private readonly Dictionary<string, EndpointInfo> _endpoints;

        public EndpointsBuilder(Dictionary<string, EndpointInfo> endpoints, string prefix)
        {
            _endpoints = endpoints;
            _prefix = prefix;
        }

        public void Map<T>(T t, string pattern, string method, Expression<Action> methodSelector) where T: ControllerBase
        {
            var methodInfo = ((MethodCallExpression)methodSelector.Body).Method;

            _endpoints.Add(_prefix+pattern, new EndpointInfo(methodInfo, method));
        }
    
    }

    private record EndpointInfo(MethodInfo Method, string RequestMethod);

    [GeneratedRegex("{([^{}]+)}")]
    private static partial Regex MyRegex();
}
