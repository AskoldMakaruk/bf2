using System.Reflection;

namespace BF2;

public class FromServicesAttribute : Attribute
{
}

public static class TypeHelper
{
    public static bool IsEndpointAttribute(this Type type) => type.IsAssignableTo(typeof(IEndpointAttribute));

    public static IEnumerable<MethodInfo> GetMethods(this BotController controller)
    {
        var methods = controller.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(a => a.CustomAttributes.Any(x => x.AttributeType.IsEndpointAttribute()));
        return methods;
    }

    public static IEnumerable<Endpoint> GetEndpoints(this BotController controller)
    {
        foreach (var method in controller.GetMethods())
        {
            var attributeType = method.CustomAttributes.FirstOrDefault(a => a.AttributeType.IsEndpointAttribute());
            if (attributeType is null)
            {
                continue;
            }

            var attribute = method.GetCustomAttributes(attributeType.AttributeType, true).FirstOrDefault(a => a.GetType().IsEndpointAttribute());
            if (attribute is not IEndpointAttribute endpointAttribute)
            {
                continue;
            }

            yield return new Endpoint(endpointAttribute, method);
        }
    }

    private static Assembly[] GetAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.FullName.Contains("Microsoft")
                        && !a.FullName.Contains("System"))
            .ToList();

        var referenced = assemblies.SelectMany(a => a.GetReferencedAssemblies())
            .Where(a => !a.FullName.Contains("Microsoft")
                        && !a.FullName.Contains("System"))
            .Select(Assembly.Load);


        return assemblies.Concat(referenced).ToArray();
    }

    public static IEnumerable<Type> GetTypes()
    {
        return GetAssemblies().SelectMany(a => a.GetTypes());
    }

    public static List<Type> GetControllerTypes()
    {
        var allTypes = GetTypes();
        var res = allTypes.Where(p => p.IsAssignableTo(typeof(BotController)) && !p.IsAbstract)
            .ToList();
        return res;
    }
}