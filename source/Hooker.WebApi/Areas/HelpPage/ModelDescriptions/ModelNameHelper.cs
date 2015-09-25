using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using static System.String;

namespace Hooker.WebApi.Areas.HelpPage.ModelDescriptions
{
    internal static class ModelNameHelper
    {
        // Modify this to provide custom model name mapping.
        public static string GetModelName(Type type)
        {
            ModelNameAttribute modelNameAttribute = type.GetCustomAttribute<ModelNameAttribute>();
            if (!IsNullOrEmpty(modelNameAttribute?.Name))
            {
                return modelNameAttribute.Name;
            }

            string modelName = type.Name;
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: GenericOfAgurment1AndArgument2
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string genericTypeName = genericType.Name;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                string[] argumentTypeNames = genericArguments.Select(GetModelName).ToArray();
                modelName = Format(CultureInfo.InvariantCulture, "{0}Of{1}", genericTypeName, Join("And", argumentTypeNames));
            }

            return modelName;
        }
    }
}