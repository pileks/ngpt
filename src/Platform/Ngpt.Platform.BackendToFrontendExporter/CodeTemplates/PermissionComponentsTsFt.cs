using Augur.BackendToFrontendExporter;
using System.Collections.Generic;
using System.Linq;
using Augur.BackendToFrontendExporter.Extensions;

namespace Ngpt.Platform.BackendToFrontendExporter.CodeTemplates
{
    public class PermissionComponentsTsFt
    {
        private readonly ComponentsModel model;

        public PermissionComponentsTsFt(ComponentsModel model)
        {
            this.model = model;
        } 

        private string GetComponentTemplate(ComponentModel model)
        {
            return $@"
                 {model.Name.ToCamelCase()} = {{
                    _componentName: '{model.Name.ToPascalCase()}',
                    {
                        model.Activities.Select(a => $@"
                            {a.ToCamelCase()}: {{
                            _componentName: '{model.Name.ToPascalCase()}',
                            _activityName: '{a.ToPascalCase()}'
                        }}
                        ".ProcessTemplate().JoinLinesBy("                        "))
                            .JoinBy("\r\n")
                    }
                }}
            ".ProcessTemplate();
        }

        public string GenerateText()
        {
            return $@"
                export class Components {{
                    
                    {
                        this.model.Components
                            .Select(c => this.GetComponentTemplate(c).JoinLinesBy("                    "))
                            .JoinBy("\r\n                    ")
                    }
                }}
                        ".ProcessTemplate();
        }

        public class ComponentModel
        {
            public string Name { get; set; }
            public IEnumerable<string> Activities { get; set; }
        }

        public class ComponentsModel
        {
            public IEnumerable<ComponentModel> Components { get; set; }
        }
    }
}
