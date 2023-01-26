using System;
using System.Collections.Generic;
using System.Reflection;

namespace Augur.BackendToFrontendExporter.Extensions
{
    public static class MethodInfoExtensions
    {
        public static IEnumerable<CustomAttributeData> GetCustomDataAttributes(this MethodInfo methodInfo,
            bool inherited)
        {
            Type lastDeclaringType = null;
            var returnedAttributeNamesDict = new Dictionary<string, bool>();

            for
            (var currentMethodInfo = methodInfo;
                lastDeclaringType == null || currentMethodInfo.DeclaringType.FullName != lastDeclaringType.FullName;
                currentMethodInfo = currentMethodInfo.GetBaseDefinition()
            )
            {
                foreach (var attributeData in currentMethodInfo.CustomAttributes)
                {
                    if (!returnedAttributeNamesDict.ContainsKey(attributeData.AttributeType.FullName))
                    {
                        returnedAttributeNamesDict.Add(attributeData.AttributeType.FullName, true);

                        yield return attributeData;
                    }
                }

                lastDeclaringType = currentMethodInfo.DeclaringType;
            }
        }
    }
}