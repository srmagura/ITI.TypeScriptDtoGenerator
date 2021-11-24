using System.Reflection;
using System.Text;
using Namotion.Reflection;

namespace ITI.TypeScriptDtoGenerator
{
    internal static class DtoGenerator
    {
        public static void GenerateDtos(
            List<Type> dtoTypes,
            List<Type> couldImportTypes,
            string? imports,
            string outputPath
        )
        {
            foreach (var type in dtoTypes)
            {
                var otherDtoTypes = dtoTypes.Where(t => t != type).ToList();
                var couldImportTypes2 = otherDtoTypes.Concat(couldImportTypes).ToList();

                GenerateDto(type, couldImportTypes2, imports, outputPath);
            }
        }

        public static void GenerateDto(Type type, List<Type> couldImportTypes, string? imports, string outputPath)
        {
            var dtoString = GenerateDtoToString(type, couldImportTypes, imports);

            var filePath = Path.Combine(outputPath, $"{type.Name}.ts");
            File.WriteAllText(filePath, dtoString);
        }

        internal static string GenerateDtoToString(Type type, List<Type> couldImportTypes, string? imports)
        {
            var interfaceBuilder = new StringBuilder();
            var unknownTypes = new List<Type>();

            var extendsClause = "";
            if (type.BaseType != null && type.BaseType.Name != "Object")
            {
                var baseType = MapType(type.BaseType.ToContextualType(), unknownTypes);
                extendsClause += $" extends {baseType}";
            }

            interfaceBuilder.AppendLine($"export interface {type.Name}{extendsClause} {{");

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                var propertyName = MapPropertyName(property.Name);

                var contextualType = property.ToContextualProperty();
                var propertyType = MapType(contextualType, unknownTypes);

                interfaceBuilder.AppendLine($"    {propertyName}: {propertyType}");
            }

            interfaceBuilder.AppendLine($"}}");

            var output = new StringBuilder();
            output.AppendLine(Util.AutoGeneratedMessage);

            if (imports != null)
                output.Append(imports);

            output.Append(GenerateImports(couldImportTypes, unknownTypes));
            output.AppendLine();
            output.Append(interfaceBuilder);
            Util.ConvertLineEndings(output);

            return output.ToString();
        }

        private static string GenerateImports(List<Type> couldImportTypes, List<Type> unknownTypes)
        {
            var couldImportTypeNames = couldImportTypes.Select(t => t.Name).ToHashSet();
            var unknownTypeNames = unknownTypes.Select(t => t.Name).ToHashSet();
            var typeNamesToImport = unknownTypeNames.Intersect(couldImportTypeNames);

            var output = new StringBuilder();

            foreach (var typeName in typeNamesToImport.OrderBy(n => n))
            {
                output.AppendLine($"import {{ {typeName} }} from './{typeName}'");
            }

            return output.ToString();
        }

        private static string MapPropertyName(string name)
        {
            return char.ToLowerInvariant(name[0]) + name[1..];
        }

        internal static string MapType(ContextualType contextualType, List<Type> unknownTypes)
        {
            var type = contextualType.Type;
            var typeName = contextualType.Type.Name;

            var rewrites = new Dictionary<string, string>
            {
                ["String"] = "string",
                ["Boolean"] = "boolean",
                ["Int32"] = "number",
                ["Int64"] = "number",
                ["Float"] = "number",
                ["Single"] = "number",
                ["Double"] = "number",
                ["Decimal"] = "number",
                ["DateTime"] = "string",
                ["DateTimeOffset"] = "string",
                ["TimeSpan"] = "string",
                ["Guid"] = "string"
            };

            if (rewrites.ContainsKey(typeName))
            {
                typeName = rewrites[typeName];
            }

            if (type.FullName != null && type.FullName.Contains("System.Collections.Generic.List"))
            {
                var genericArgumentTypeName = MapType(contextualType.GenericArguments.Single(), unknownTypes);

                if (genericArgumentTypeName.Contains(' '))
                {
                    typeName = $"({genericArgumentTypeName})[]";
                }
                else
                {
                    typeName = genericArgumentTypeName + "[]";
                }
            }
            else if (type.IsGenericType)
            {
                var genericArgumentNames = contextualType.GenericArguments.Select(t => MapType(t, unknownTypes));

                typeName = typeName.Split('`')[0];
                typeName += "<" + string.Join(", ", genericArgumentNames) + ">";
            }
            else
            {
                if (typeName != "number" && typeName != "string" && typeName != "boolean")
                {
                    unknownTypes.Add(type);
                }
            }

            if (contextualType.Nullability == Nullability.Nullable)
                typeName += " | null | undefined";

            return typeName;
        }
    }
}
