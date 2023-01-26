using Augur.BackendToFrontendExporter;
using System.Linq;
using Augur.BackendToFrontendExporter.Extensions;
using System;
using System.Reflection;
using Ngpt.Platform.Data.Attributes;

namespace Ngpt.Platform.BackendToFrontendExporter.CodeTemplates
{
    public class EnumTsFt
    {
        private readonly TypeDescription model;

        public EnumTsFt(TypeDescription model)
        {
            this.model = model;
        }

        public string GenerateText()
        {
            var enumNames = Enum.GetNames(model.Type);

            return $@"
                export enum {this.model.Type.Name.ToPascalCase()} {{
                    {
                        Enum.GetValues(this.model.Type)
                            .Cast<int>()
                            .Select(value => $"{Enum.GetName(this.model.Type, value)} = {value}")
                            .JoinBy(",\r\n                    ")
                    }
                }}
                
                export const {this.model.Type.Name.ToPascalCase()}Definition = {{
                    {
                        enumNames.Select(name => {
                            var value = (int)Enum.Parse(model.Type, name);
                            var titleAttr = model.Type.GetMember(name).First().GetCustomAttribute<TitleAttribute>();
                            var title = titleAttr != null ? titleAttr.Title : name;

                            return 
                    $@"{value}: {{
                        title: '{title}',
                        value: {this.model.Type.Name.ToPascalCase()}.{name}
                    }}";
                            })
                        .JoinBy(",\r\n                    ")
                    }
                }}
                        ".ProcessTemplate();
        }
    }

}
