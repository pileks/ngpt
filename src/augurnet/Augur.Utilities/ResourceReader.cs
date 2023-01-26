using System.IO;
using System.Reflection;

namespace Augur.Utilities
{
    public static class ResourceReader
    {
        public static string Read(Assembly assembly, string resourceName)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}