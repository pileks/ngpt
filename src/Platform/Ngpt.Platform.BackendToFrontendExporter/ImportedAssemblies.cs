using System;
using Microsoft.CodeAnalysis;

namespace Ngpt.Platform.BackendToFrontendExporter
{
    public static class ImportedAssemblies
    {
        public static string[] GetApiControllerAssemblyNames()
        {
            return new[]
            {
                "Ngpt.Web",
                "Ngpt.Platform.Web",
                "Ngpt.Platform.AccessControl.Web",
                "Ngpt.Platform.FileResources.Web",
                "Ngpt.Platform.Identity.Web"
            };
        }

        public static Type[] GetApiControllerAssemblyTokens()
        {
            return new[]
            {
                typeof(Ngpt.Web.AssemblyToken),
                typeof(Platform.Web.AssemblyToken),
                typeof(Platform.FileResources.Web.AssemblyToken),
                typeof(Platform.Identity.Web.AssemblyToken)
            };
        }
    }
}