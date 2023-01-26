using Augur.Web;
using Augur.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Augur.BackendToFrontendExporter.Extensions;
using Augur.BackendToFrontendExporter;

namespace Ngpt.Platform.BackendToFrontendExporter.CodeTemplates
{
    public class AugurEntityControllerTsFt
    {
        private readonly Model model;
        private readonly IList<string> baseEndpointNames;

        public AugurEntityControllerTsFt(Model model, IList<string> baseEndpointNames)
        {
            this.model = model;
            this.baseEndpointNames = baseEndpointNames;
        }

        private string GetEndpointTemplates(EndpointModel endpoint)
        {
            var hasCustomHeaders = endpoint.Info.GetCustomAttribute<ExportToFrontendWithCustomHeadersAttribute>() != null;
            var hasResponseType = endpoint.Info.GetCustomAttribute<ExportToFrontendWithResponseTypeAttribute>() != null;

            var returnType = GetEndpointReturnType(endpoint, out var hasValidation);

            var returnTypeStr = returnType != null
                ? $"AugurHttpRequest<{GetReturnTypeStr(returnType.ToDescriptionModel().TypescriptType, hasCustomHeaders)}>"
                : "IAugurHttpRequest";

            var genericReturnModel = returnType != null
                ? $"<{GetReturnTypeStr(returnType.ToDescriptionModel().TypescriptType, hasCustomHeaders)}>"
                : "";

            var convertToAugurHttpRequest = hasValidation
                ? $".toAugurHttpRequestWithValidation{genericReturnModel}();"
                : $".toAugurHttpRequest{genericReturnModel}();";

            var endpointRoute = GetEndpointRoute(endpoint, out var routeParams);

            var template = $@"
                    {endpoint.Name.ToCamelCase()}({GetEndpointParameters(endpoint, routeParams)}): {returnTypeStr} {{
                        {GetQueryParamsDeclaration(endpoint, routeParams).SplitBy("\r\n").JoinBy("\r\n                        ")}
                        return this.http.{endpoint.Type.ToCamelCase()}(this.baseUrl + this.controllerRoute{endpointRoute}{GetHttpParameters(endpoint, routeParams, hasCustomHeaders, hasResponseType)})
                            {convertToAugurHttpRequest}
                    }}
                ".ProcessTemplate();

            return template;
        }

        private string GetReturnTypeStr(string returnTypeName, bool hasCustomHeaders)
        {
            return hasCustomHeaders 
                ? $"HttpResponse<{returnTypeName}>"
                : returnTypeName;
        }

        private Type GetEndpointReturnType(EndpointModel endpoint, out bool hasValidation)
        {
            hasValidation = false;

            if (endpoint.ReturnType.BaseType != null)
            {
                if (endpoint.ReturnType.Name == typeof(Task<>).Name)
                {
                    var validationResultType = endpoint.ReturnType.GenericTypeArguments.Single();

                    return GetEndpointReturnType(validationResultType, out hasValidation);
                }
                else
                {
                    return GetEndpointReturnType(endpoint.ReturnType, out hasValidation);
                }
            }
            else
            {
                return GetEndpointReturnType(endpoint.ReturnType, out hasValidation);
            }
        }

        private Type GetEndpointReturnType(Type type, out bool hasValidation)
        {
            hasValidation = false;

            if (type.Name == typeof(ValidationResult<>).Name)
            {
                var modelType = type.GenericTypeArguments.Single();

                hasValidation = true;

                return modelType;
            }
            else
            {
                if (typeof(IValidationResult).IsAssignableFrom(type))
                {
                    hasValidation = true;
                }

                if (type == typeof(ValidationResult))
                {
                    return null;
                }
                else if (type == typeof(IActionResult))
                {
                    return null;
                }
                else if (type.Name == typeof(ActionResult<>).Name)
                {
                    var modelType = type.GenericTypeArguments.Single();

                    return modelType;
                }
                else
                {
                    return type;
                }
            }
        }

        private string GetEndpointParameters(EndpointModel endpoint, IList<string> routeParams)
        {
            return endpoint.Info.GetParameters().Any()
                ? endpoint.Info.GetParameters()
                    .SelectMany(p =>
                    {
                        if (p.GetCustomAttribute<FromBodyAttribute>() == null && !routeParams.Contains(p.Name))
                        {
                            if (p.ParameterType.IsSimpleType())
                            {
                                return new[]
                                {
                                    new
                                    {
                                        Name = p.Name.ToCamelCase(),
                                        NullableString = p.ParameterType.ToDescriptionModel().NullableString,
                                        Type = p.ParameterType.ToDescriptionModel().TypescriptType
                                    }
                                };
                            }
                            else
                            {
                                return p.ParameterType
                                    .GetProperties()
                                    .Select(pr => new
                                    {
                                        Name = pr.Name.ToCamelCase(),
                                        NullableString = pr.PropertyType.ToDescriptionModel().NullableString,
                                        Type = pr.PropertyType.ToDescriptionModel().TypescriptType
                                    });
                            }
                        }
                        else
                        {
                            return new[]
                            {
                                new
                                {
                                    Name = p.Name.ToCamelCase(),
                                    NullableString = p.ParameterType.ToDescriptionModel().NullableString,
                                    Type = p.ParameterType.ToDescriptionModel().TypescriptType
                                }
                            };
                        }
                    })
                    .Select(p => $"{p.Name}{p.NullableString}: {p.Type}")
                    .JoinBy(", ")
                : "";
        }

        private string GetHttpParameters(EndpointModel endpoint, IList<string> routeParams, bool hasCustomHeaders, bool hasResponseType)
        {
            var bodyParamInfo = endpoint.Info.GetParameters().SingleOrDefault(p => p.GetCustomAttribute<FromBodyAttribute>() != null);

            var bodyParam = "";

            if (bodyParamInfo == null && endpoint.Type == "Post")
            {
                bodyParam = ", null";
            }
            else if (bodyParamInfo != null)
            {
                bodyParam = $", {bodyParamInfo.Name.ToCamelCase()} ? {bodyParamInfo.Name.ToCamelCase()} : {{}}";
            }

            var responseType = endpoint.Info.GetCustomAttribute<ExportToFrontendWithResponseTypeAttribute>()?.ResponseType;

            var options = new[]
            {
                GetQueryParams(endpoint, routeParams),
                hasCustomHeaders ? "observe: 'response'" : null,
                hasResponseType ? $"responseType: '{responseType}'" : null
            }
            .Where(x => x != null)
            .JoinBy(", ");

            options = !string.IsNullOrEmpty(options) ? $", {{ {options} }}" : "";

            return $"{bodyParam}{options}";
        }

        private string GetQueryParams(EndpointModel endpoint, IList<string> routeParams)
        {
            return endpoint.Info.GetParameters()
                .Where(p => p.GetCustomAttribute<FromBodyAttribute>() == null)
                .Where(p => !routeParams.Contains(p.Name))
                .Any()
                    ? $"params: queryParams"
                    : null;
        }

        private string GetQueryParamsDeclaration(EndpointModel endpoint, List<string> routeParams)
        {
            var queryParams = endpoint.Info.GetParameters()
                .Where(p => p.GetCustomAttribute<FromBodyAttribute>() == null)
                .Where(p => !routeParams.Contains(p.Name))
                .SelectMany(p =>
                {
                    if (p.ParameterType.IsSimpleType())
                    {
                        return new[] { p.Name.ToCamelCase() };
                    }
                    else
                    {
                        return p.ParameterType
                            .GetProperties()
                            .Select(pr => pr.Name.ToCamelCase());
                    }
                })
                .ToList();

            var test = queryParams.Any()
                    ? $@"
                            const queryParams = new HttpParams()
                                {
                                    queryParams.Select(qp => $".set('{qp}', {qp} ? {qp}.toString() : '')").JoinBy("\r\n                                ")
                                };
                            
                    ".ProcessTemplate()
                    : "";

            return test;
        }

        private string GetEndpointRoute(EndpointModel endpoint, out List<string> routeParams)
        {
            routeParams = new List<string>();
            var endpointRoute = "";
            var routeTemplateArgument = endpoint.HttpAttribute.ConstructorArguments.SingleOrDefault();

            if (routeTemplateArgument != null)
            {
                var routeTemplate = routeTemplateArgument.Value as string;
                if (routeTemplate != null)
                {
                    var compiledRoute = routeTemplate
                        .SplitBy("/")
                        .Select(p =>
                        {
                            if (p.Contains("{"))
                            {
                                var paramName = p.GetBetween("{", "}").ToCamelCase();

                                return $"${{{ paramName }}}";
                            }
                            else
                            {
                                return p.ToCamelCase();
                            }
                        })
                        .JoinBy("/");

                    var queryParamNames = routeTemplate
                        .SplitBy("/")
                        .Where(p => p.Contains("{"))
                        .Select(p => p.GetBetween("{", "}").ToCamelCase())
                        .ToList();

                    if (queryParamNames.Any())
                    {
                        routeParams.AddRange(queryParamNames);
                    }

                    endpointRoute = $" + `/{compiledRoute}`";
                }
            }

            return endpointRoute;
        }

        public string GenerateText()
        {
            var types = this.model.Endpoints
                        .Where(e => !this.baseEndpointNames.Contains(e.Name))
                        .SelectMany(e => e.Info.GetParameters())
                        .Where(p => p.ParameterType.IsSimpleType() || p.GetCustomAttribute<FromBodyAttribute>() != null)
                        .Select(p => p.ParameterType.ToDescriptionModel())
                        .Concat
                        (
                            this.model.Endpoints
                                .Where(e => !this.baseEndpointNames.Contains(e.Name))
                                .Select(e => GetEndpointReturnType(e, out var hasValidation))
                                .Where(t => t != null)
                                .Select(t => t.ToDescriptionModel())
                        )
                        .Append(this.model.Entity)
                        .Where(m => m.IsModelType)
                        .Select(m => m.IsCollection ? m.GenericArgument : m)
                        .DistinctBy(m => m.Type)
                        .ToList();

            return $@"
                import {{ HttpClient, HttpParams, HttpResponse }} from '@angular/common/http';
                import {{ Inject, Injectable }} from '@angular/core';
                import {{ AugurEntityController, AugurHttpRequest, IAugurHttpRequest }} from '@augur';
                {
                    types
                        .Select(m => new
                        {
                            Type = m,
                            Path = m.GetTypescriptImportPath()
                        })
                        .Select(m => $"import {{ { m.Type.TypescriptType } }} from '{m.Path}';")
                        .JoinBy("\r\n                ")
                }
                
                @Injectable({{
                    providedIn: 'root'
                }})
                export class {this.model.Name} extends AugurEntityController<{this.model.Entity.TypescriptType}> {{
                
                    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {{
                        super('api/{ this.model.Name.Split(new[] { "Controller" }, StringSplitOptions.None)[0].ToCamelCase() }', http, baseUrl);
                        
                        {
                            this.model.Endpoints
                                .Where(e => !this.baseEndpointNames.Contains(e.Name))
                                .Select(e => $"this.{e.Name.ToCamelCase()} = this.{e.Name.ToCamelCase()}.bind(this);")
                                .JoinBy("\r\n                        ")
                        }
                    }}
                    
                    {
                        this.model.Endpoints
                            .Where(e => !this.baseEndpointNames.Contains(e.Name))
                            .Select
                            (   
                                e => this.GetEndpointTemplates(e)
                                    .JoinLinesBy("                    ")
                            )
                            .JoinBy("\r\n                    \r\n                    ")
                    }
                }}
                        ".ProcessTemplate();
        }

        public class Model
        {
            public string Name { get; set; }
            public TypeDescription Entity { get; set; }
            public ICollection<EndpointModel> Endpoints { get; set; }
        }

        public class EndpointModel
        {
            public string Name { get; set; }
            public CustomAttributeData HttpAttribute { get; set; }
            public string Type { get; set; }
            public MethodInfo Info { get; set; }
            public Type ReturnType { get; set; }
        }
    }
}
