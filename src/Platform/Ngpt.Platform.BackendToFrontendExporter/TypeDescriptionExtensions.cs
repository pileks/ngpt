using System;
using Augur.BackendToFrontendExporter;
using Augur.BackendToFrontendExporter.Extensions;
using System.Linq;

namespace Ngpt.Platform.BackendToFrontendExporter
{
    public static class TypeDescriptionExtensions
    {
        public static string GetTypescriptImportPath(this TypeDescription typeDescription)
        {
            var path = typeDescription switch
            {
                TypeDescription t when t.IsModelType && t.Type.IsEnum                                                    
                    => $"{GetBaseAppPath(t.Type)}/enums",
                TypeDescription t when t.IsModelType && !t.Type.IsEnum && ImportedAssemblies.GetApiControllerAssemblyNames().Any(x => t.Type.Namespace.StartsWith($"{x}.Models")) 
                    => $"{GetBaseAppPath(t.Type)}/models",
                TypeDescription t when t.IsModelType && !t.Type.IsEnum && t.Type.Namespace.EndsWith(".Models")            
                    => GetImportPathForControllerModel(typeDescription),
                TypeDescription t => $"{GetBaseAppPath(t.Type)}/entities"
            };

            return $"{path}/{typeDescription.TypescriptType.ToDashCase()}";
        }

        private static string GetImportPathForControllerModel(TypeDescription typeDescription)
        {
            var controllerDirectory = typeDescription.Type.Namespace
                .SplitBy(".")
                .ToList()[^2];

            return $"{GetBaseAppPath(typeDescription.Type)}/{GetApiControllerPath(typeDescription.Type)}/{controllerDirectory.ToDashCase()}/models";
        }

        private static string GetBaseAppPath(Type type)
        {
            if (type.Namespace.StartsWith("Ngpt.Platform"))
            {
                return "@platform";
            }
            else
            {
                return "@src/app";
            }
        }

        private static string GetApiControllerPath(Type type)
        {
            if (type.Namespace.StartsWith("Ngpt.Platform"))
            {
                var module = type.Namespace.SplitBy(".").ToList()[2].ToDashCase();

                return $@"web-api-controllers/{module}";
            }
            else
            {
                return @"web-api-controllers";
            }
        }
    }
}
