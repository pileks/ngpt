using Augur.BackendToFrontendExporter;
using System.Collections.Generic;
using System.Linq;
using Augur.BackendToFrontendExporter.Extensions;

namespace Ngpt.Platform.BackendToFrontendExporter.CodeTemplates
{
    public class ApiControllerModelTsFt
    {
        private readonly TypeDescription model;

        public ApiControllerModelTsFt(TypeDescription model)
        {
            this.model = model;
        }

        private string GetImportTemplates(TypeDescription model)
        {
            return $@"
                        import {{ {model.Type.Name} }} from '{model.GetTypescriptImportPath()}';
                    ".ProcessTemplate();
        }

        private IEnumerable<string> GetPropertyTemplate(PropertyDescription model)
        {
            return new string[]
            {
                $@"
                    {model.Name.ToCamelCase()}{model.Type.NullableString}: {model.Type.TypescriptType};
                ".ProcessTemplate()
            };
        }

        public string GenerateText()
        {
            var separator = this.model.Properties
                        .Where(p => p.Type.IsModelType)
                        .Any()
                ? "\r\n                \r\n                "
                : "";

            return $@"
                {
                    this.model.Properties
                        .Where(p => p.Type.IsModelType)
                        .Select(p => p.Type)
                        .Select(m => m.IsCollection ? m.GenericArgument : m)
                        .DistinctBy(m => m.Type)
                        .Where(m => m.Type.FullName != this.model.Type.FullName)
                        .Select(m => this.GetImportTemplates(m))
                        .JoinBy("\r\n                ")
                }{ separator }export class {this.model.Type.Name.ToPascalCase()} {{
                    {
                        this.model.Properties
                            .SelectMany(p => this.GetPropertyTemplate(p))
                            .JoinBy("\r\n                    ")
                    }
                }}
                        ".ProcessTemplate();
        }
    }

}
