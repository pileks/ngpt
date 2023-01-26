using System;
using System.Reflection;

namespace Augur.BackendToFrontendExporter.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return typeInfo.GetGenericArguments()[0].IsSimpleType();
            }

            return typeInfo.IsPrimitive
                   || typeInfo.IsValueType
                   || typeInfo.IsEnum
                   || typeInfo.Equals(typeof(string))
                   || typeInfo.Equals(typeof(decimal));
        }

        public static TypeDescription ToDescriptionModel(this Type type)
        {
            return new TypeDescription(type);
        }
    }
}