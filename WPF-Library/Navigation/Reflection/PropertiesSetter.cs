using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLibrary.Navigation.Attributes;

namespace WPFLibrary.Navigation.Reflection;

public static class PropertiesSetter
{
    public static void SetParameters (object obj, params (string Name, object Value)[] objects)
    {
        var classType = obj.GetType();

        var attributes = classType.GetCustomAttributes(false);

        var paramAttributes = attributes.Where(a => a is ParameterAttribute)
                                        .Distinct()
                                        .Cast<ParameterAttribute>();

        if (!paramAttributes.Any())
            throw new InvalidOperationException("Class does not have Parameters Attributes");

        var props = classType.GetProperties()
                             .Where(p => p.CanWrite);

        if (!props.Any())
            throw new InvalidOperationException("No properties found with accesible set method");

        foreach (var attribute in paramAttributes) {
            var propInfo = props.Where(prop => prop.Name == attribute.Name &&
                                               prop.PropertyType == attribute.ParameterType)
                            .FirstOrDefault();

            if (propInfo is null)
                throw new NullReferenceException("No properties found with this name and type");

            var value = objects.Where(o => o.Name == attribute.Name &&
                                           o.Value.GetType() == attribute.ParameterType)
                               .Select(o => o.Value)
                               .FirstOrDefault();

            propInfo.SetValue(obj, value);
        }
    }
}