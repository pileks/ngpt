using System.Linq;
using System.Reflection;
using System;
using Augur.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Augur.BackendToFrontendExporter;
using System.IO;
using Microsoft.Extensions.Configuration;
using Augur.BackendToFrontendExporter.Extensions;
using Augur.Entity.Interfaces.Base;
using Augur.Security.Permissions.Components;
using Ngpt.Data;
using Ngpt.Platform.BackendToFrontendExporter.CodeTemplates;

namespace Ngpt.Platform.BackendToFrontendExporter
{
    class Program
    {
        static IConfiguration configuration;

        static void Main(string[] args)
        {
            Configure();

            AssembleApiControllersFile();
            AssembleApiControllerModelFiles();
            AssembleApiModelFiles();
            AssembleAugurEntityApiControllersFiles();
            AssembleAugurEntityWithGridApiControllersFiles();
            AssembleAugurEntityFiles();
            AssembleAugurEnumFiles();
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            Program.configuration = builder.Build();
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        private static void AssembleApiControllersFile()
        {
            var allControllerTypes = ImportedAssemblies.GetApiControllerAssemblyTokens()
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => typeof(AugurApiController).IsAssignableFrom(type))
                .Where(type => !IsAssignableToGenericType(type, typeof(AugurEntityController<,,>)))
                .Where(type => !type.IsAbstract)
                .Where(type => type.GetCustomAttribute<DisableExportToFrontendAttribute>() == null);

            var autogenComponents = allControllerTypes
                .SelectMany(e => new List<AutoGenComponent>
                {
                    new FileAutoGen
                    (
                        new ApiControllerTsFt(new ApiControllerTsFt.Model
                        {
                            Name = e.Name,
                            Endpoints = e.GetMethods()
                                .Where(m => m.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                                .Where(m =>
                                    m.GetCustomAttribute<HttpGetAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPostAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPutAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPatchAttribute>() != null ||
                                    m.GetCustomAttribute<HttpDeleteAttribute>() != null)
                                .Select(m => new ApiControllerTsFt.EndpointModel
                                {
                                    Name = m.Name,
                                    HttpAttribute = m.GetCustomDataAttributes(inherited: true)
                                        .Single(a =>
                                            a.AttributeType.Name == "HttpGetAttribute" ||
                                            a.AttributeType.Name == "HttpPostAttribute" ||
                                            a.AttributeType.Name == "HttpPutAttribute" ||
                                            a.AttributeType.Name == "HttpPatchAttribute" ||
                                            a.AttributeType.Name == "HttpDeleteAttribute"),
                                    Type = GetAttributeType(m.GetCustomDataAttributes(inherited: true)
                                        .Single(a =>
                                            a.AttributeType.Name == "HttpGetAttribute" ||
                                            a.AttributeType.Name == "HttpPostAttribute" ||
                                            a.AttributeType.Name == "HttpPutAttribute" ||
                                            a.AttributeType.Name == "HttpPatchAttribute" ||
                                            a.AttributeType.Name == "HttpDeleteAttribute")),
                                    Info = m,
                                    ReturnType = m.ReturnType
                                })
                                .ToList()
                         }).GenerateText(),
                        $@"{GetFrontendPath(e)}\{GetApiControllerPath(e)}",
                        $"{e.Name.Split(new[] { "Controller" }, StringSplitOptions.None)[0].ToDashCase()}/{e.Name.ToDashCase()}.ts"
                    )
                });

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static string GetBaseFrontendPath()
        {
            return $@"{Program.configuration["SolutionDirectory"]}\Ngpt.Web\ClientApp\src";
        }

        private static string GetFrontendPlatformPath()
        {
            return $@"{GetBaseFrontendPath()}\platform";
        }

        private static string GetFrontendPath(Type type)
        {
            if (type.Namespace.StartsWith("Ngpt.Platform"))
            {
                return $@"{GetBaseFrontendPath()}\platform";
            }
            else
            {
                return $@"{GetBaseFrontendPath()}\app";
            }
        }

        private static string GetApiControllerPath(Type type)
        {
            if (type.Namespace.StartsWith("Ngpt.Platform"))
            {
                var module = type.Namespace.SplitBy(".").ToList()[2].ToDashCase();

                return $@"web-api-controllers\{module}";
            }
            else
            {
                return @"web-api-controllers";
            }
        }

        private static void AssembleAugurEntityApiControllersFiles()
        {
            var allControllerTypes = ImportedAssemblies.GetApiControllerAssemblyTokens()
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => IsAssignableToGenericType(type, typeof(AugurEntityController<,,>)))
                .Where(type => !IsAssignableToGenericType(type, typeof(AugurEntityWithGridController<,>)))
                .Where(type => !type.IsAbstract)
                .Where(type => type.GetCustomAttribute<DisableExportToFrontendAttribute>() == null);

            var autogenComponents = allControllerTypes
                .SelectMany(e => new List<AutoGenComponent>
                {
                    new FileAutoGen
                    (
                        new AugurEntityControllerTsFt(new AugurEntityControllerTsFt.Model
                        {
                            Name = e.Name,
                            Entity = e.BaseType.GenericTypeArguments.First().ToDescriptionModel(),
                            Endpoints = e.GetMethods()
                                .Where(m => m.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                                .Where(m =>
                                    m.GetCustomAttribute<HttpGetAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPostAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPutAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPatchAttribute>() != null ||
                                    m.GetCustomAttribute<HttpDeleteAttribute>() != null)
                                .Select(m => new AugurEntityControllerTsFt.EndpointModel
                                {
                                    Name = m.Name,
                                    HttpAttribute = m.GetCustomDataAttributes(inherited: true)
                                        .Single(a =>
                                            a.AttributeType.Name == "HttpGetAttribute" ||
                                            a.AttributeType.Name == "HttpPostAttribute" ||
                                            a.AttributeType.Name == "HttpPutAttribute" ||
                                            a.AttributeType.Name == "HttpPatchAttribute" ||
                                            a.AttributeType.Name == "HttpDeleteAttribute"),
                                    Type = GetAttributeType(m.GetCustomDataAttributes(inherited: true)
                                        .Single(a =>
                                            a.AttributeType.Name == "HttpGetAttribute" ||
                                            a.AttributeType.Name == "HttpPostAttribute" ||
                                            a.AttributeType.Name == "HttpPutAttribute" ||
                                            a.AttributeType.Name == "HttpPatchAttribute" ||
                                            a.AttributeType.Name == "HttpDeleteAttribute")),
                                    Info = m,
                                    ReturnType = m.ReturnType
                                })
                                .ToList()
                         }, e.BaseType.GetMethods().Select(m => m.Name).ToList()).GenerateText(),
                        $@"{GetFrontendPath(e)}\{GetApiControllerPath(e)}",
                        $"{e.Name.Split(new[] { "Controller" }, StringSplitOptions.None)[0].ToDashCase()}/{e.Name.ToDashCase()}.ts"
                    )
                });

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static void AssembleAugurEntityWithGridApiControllersFiles()
        {
            var allControllerTypes = ImportedAssemblies.GetApiControllerAssemblyTokens()
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => IsAssignableToGenericType(type, typeof(AugurEntityWithGridController<,,>)))
                .Where(type => !type.IsAbstract)
                .Where(type => type.GetCustomAttribute<DisableExportToFrontendAttribute>() == null);

            var autogenComponents = allControllerTypes
                .SelectMany(e => new List<AutoGenComponent>
                {
                    new FileAutoGen
                    (
                        new AugurEntityWithGridControllerTsFt(new AugurEntityWithGridControllerTsFt.Model
                        {
                            Name = e.Name,
                            Entity = e.BaseType.GenericTypeArguments.First().ToDescriptionModel(),
                            GridModel = e.BaseType.GenericTypeArguments.Skip(1).First().ToDescriptionModel(),
                            Endpoints = e.GetMethods()
                                .Where(m => m.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                                .Where(m =>
                                    m.GetCustomAttribute<HttpGetAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPostAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPutAttribute>() != null ||
                                    m.GetCustomAttribute<HttpPatchAttribute>() != null ||
                                    m.GetCustomAttribute<HttpDeleteAttribute>() != null)
                                .Select(m => new AugurEntityWithGridControllerTsFt.EndpointModel
                                {
                                    Name = m.Name,
                                    HttpAttribute = m.GetCustomDataAttributes(inherited: true)
                                        .Single(a =>
                                            a.AttributeType.Name == "HttpGetAttribute" ||
                                            a.AttributeType.Name == "HttpPostAttribute" ||
                                            a.AttributeType.Name == "HttpPutAttribute" ||
                                            a.AttributeType.Name == "HttpPatchAttribute" ||
                                            a.AttributeType.Name == "HttpDeleteAttribute"),
                                    Type = GetAttributeType(m.GetCustomDataAttributes(inherited: true)
                                        .Single(a =>
                                            a.AttributeType.Name == "HttpGetAttribute" ||
                                            a.AttributeType.Name == "HttpPostAttribute" ||
                                            a.AttributeType.Name == "HttpPutAttribute" ||
                                            a.AttributeType.Name == "HttpPatchAttribute" ||
                                            a.AttributeType.Name == "HttpDeleteAttribute")),
                                    Info = m,
                                    ReturnType = m.ReturnType
                                })
                                .ToList()
                         }, e.BaseType.GetMethods().Select(m => m.Name).ToList()).GenerateText(),
                        $@"{GetFrontendPath(e)}\{GetApiControllerPath(e)}",
                        $"{e.Name.Split(new[] { "Controller" }, StringSplitOptions.None)[0].ToDashCase()}/{e.Name.ToDashCase()}.ts"
                    )
                });

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static void AssembleApiControllerModelFiles()
        {
            var allControllerTypes = ImportedAssemblies.GetApiControllerAssemblyTokens()
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => typeof(AugurApiController).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Where(type => type.GetCustomAttribute<DisableExportToFrontendAttribute>() == null);

            var autogenComponents = allControllerTypes
                .SelectMany(e => ImportedAssemblies.GetApiControllerAssemblyTokens()
                                .Select(t => t.Assembly)
                                .SelectMany(a => a.GetTypes())
                                .Where(type => !type.IsEnum)
                                .Where(t => t.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                                .Where(t => t.Namespace != null && t.Namespace.Contains(".") && t.Namespace == $"{e.Namespace}.Models")
                                .Select(t => new FileAutoGen
                                (
                                    new ApiControllerModelTsFt(t.ToDescriptionModel()).GenerateText(),
                                    $@"{GetFrontendPath(e)}\{GetApiControllerPath(e)}\{ e.Name.Split(new[] { "Controller" }, StringSplitOptions.None)[0].ToDashCase()}\models",
                                    $"{t.Name.ToDashCase()}.ts"
                                 ))
                                .ToList());

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static void AssembleApiModelFiles()
        {
            var autogenComponents = ImportedAssemblies.GetApiControllerAssemblyTokens()
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => !type.IsEnum)
                .Where(t => t.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                .Where(t => t.Namespace != null && ImportedAssemblies.GetApiControllerAssemblyNames().Any(x => t.Namespace.StartsWith($"{x}.Models")))
                .Select(t => new FileAutoGen
                (
                    new ApiControllerModelTsFt(t.ToDescriptionModel()).GenerateText(),
                    $@"{GetFrontendPath(t)}\models",
                    $"{t.Name.ToDashCase()}.ts"
                    ))
                .ToList();

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static void AssembleAugurEntityFiles()
        {
            var autogenComponents = new[] { typeof(AssemblyToken), typeof(Platform.Data.AssemblyToken) }
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => typeof(IAugurEntity).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Where(type => !type.IsEnum)
                .Where(type => type.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                .Select(type => new FileAutoGen
                (
                    new EntityTsFt(type.ToDescriptionModel()).GenerateText(),
                    $@"{GetFrontendPath(type)}\entities",
                    $"{type.Name.ToDashCase()}.ts"
                ));

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static void AssembleAugurEnumFiles()
        {
            var autogenComponents = ImportedAssemblies.GetApiControllerAssemblyTokens().Concat(new[]
                {
                    typeof(Ngpt.Data.AssemblyToken),
                    typeof(Platform.Data.AssemblyToken)
                })
                .Select(t => t.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(type => typeof(Enum).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Where(type => type.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                .Select(type => new FileAutoGen
                (
                    new EnumTsFt(type.ToDescriptionModel()).GenerateText(),
                    $@"{GetFrontendPath(type)}\enums",
                    $"{type.Name.ToDashCase()}.ts"
                ));

            foreach (var component in autogenComponents)
            {
                component.Execute();
            }
        }

        private static string GetAttributeType(CustomAttributeData customAttributeData)
        {
            var attributeName = customAttributeData.AttributeType.Name;

            int pFrom = attributeName.IndexOf("Http") + "Http".Length;
            int pTo = attributeName.LastIndexOf("Attribute");

            return attributeName.Substring(pFrom, pTo - pFrom);
        }
    }
}
