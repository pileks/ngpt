using Augur.BackendToFrontendExporter.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Augur.BackendToFrontendExporter
{
    public class TypeDescription
    {
        public TypeDescription(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; set; }

        private TypeDescription baseDesc;

        public TypeDescription Base
        {
            get
            {
                if (this.baseDesc == null)
                {
                    this.baseDesc = this.Type.BaseType?.ToDescriptionModel();
                }

                return this.baseDesc;
            }
        }

        private ICollection<PropertyDescription> properties;

        public ICollection<PropertyDescription> Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = this.Type.GetProperties()
                        .Where(p => p.GetCustomAttribute<DisableExportToFrontendAttribute>() == null)
                        .Select(p => new PropertyDescription
                        {
                            Name = p.Name,
                            Type = p.PropertyType.ToDescriptionModel()
                        })
                        .ToList();
                }

                return this.properties;
            }
        }

        public bool IsCollection
        {
            get
            {
                if (typeof(IEnumerable).IsAssignableFrom(this.Type) && this.Type != typeof(string))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public TypeDescription GenericArgument
        {
            get
            {
                if (this.IsCollection)
                {
                    return new TypeDescription(this.Type.GenericTypeArguments.Single());
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsNullableValueType
        {
            get
            {
                if (this.Type.Name == "Nullable`1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string NullableString
        {
            get
            {
                if (this.IsNullableValueType)
                {
                    return "?";
                }
                else
                {
                    return "";
                }
            }
        }

        public bool IsModelType
        {
            get
            {
                var name = this.Type.Name;

                switch (name)
                {
                    case "String":
                        return false;

                    case "Int32":
                        return false;

                    case "Int64":
                        return false;

                    case "Single":
                        return false;

                    case "Decimal":
                        return false;

                    case "Double":
                        return false;

                    case "Boolean":
                        return false;

                    case "DateTime":
                        return false;

                    default:

                        if (this.Type.IsEnum)
                        {
                            return true;
                        }
                        else if (this.IsCollection)
                        {
                            return this.Type.GenericTypeArguments.Single().ToDescriptionModel().IsModelType;
                        }
                        else
                        {
                            if (this.Type.IsInterface)
                            {
                                return false;
                            }
                            else if (this.Type == typeof(object))
                            {
                                return false;
                            }
                            else if (this.Type.Name == "Nullable`1")
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                }
            }
        }

        public string CSharpType
        {
            get
            {
                var name = this.Type.Name;

                switch (name)
                {
                    case "String":
                        return "string";

                    case "Int32":
                        return "int";

                    case "Int64":
                        return "long";

                    case "Single":
                        return "float";

                    case "Decimal":
                        return "decimal";

                    case "Double":
                        return "double";

                    case "Boolean":
                        return "bool";

                    case "DateTime":
                        return "DateTime";

                    default:

                        if (this.Type.IsEnum)
                        {
                            return name;
                        }
                        else if (this.IsCollection)
                        {
                            return
                                $"IEnumerable<{this.Type.GenericTypeArguments.Single().ToDescriptionModel().CSharpType}>";
                        }
                        else if (this.Type == typeof(object))
                        {
                            return "object";
                        }
                        else if (this.Type.Name == "Nullable`1")
                        {
                            return $"{this.Type.GenericTypeArguments.Single().ToDescriptionModel().TypescriptType}";
                        }
                        else
                        {
                            return name;
                        }
                }
            }
        }

        public string TypescriptType
        {
            get
            {
                var name = this.Type.Name;

                switch (name)
                {
                    case "String":
                        return "string";

                    case "Int32":
                    case "Int64":
                    case "Single":
                    case "Decimal":
                    case "Double":
                        return "number";

                    case "Boolean":
                        return "boolean";

                    case "DateTime":
                        return "Date";

                    default:

                        if (this.Type.IsEnum)
                        {
                            return name;
                        }
                        else if (this.IsCollection)
                        {
                            return $"{this.Type.GenericTypeArguments.Single().ToDescriptionModel().TypescriptType}[]";
                        }
                        else
                        {
                            if (this.Type.IsInterface)
                            {
                                return "any";
                            }
                            else if (this.Type == typeof(object))
                            {
                                return "any";
                            }
                            else if (this.Type.Name == "Nullable`1")
                            {
                                return $"{this.Type.GenericTypeArguments.Single().ToDescriptionModel().TypescriptType}";
                            }
                            else
                            {
                                return name;
                            }
                        }
                }
            }
        }
    }
}