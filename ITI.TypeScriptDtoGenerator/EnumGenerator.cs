﻿using System.Text;

namespace ITI.TypeScriptDtoGenerator
{
    internal static class EnumGenerator
    {
        public static void GenerateEnum(Type type, string outputPath)
        {
            var output = new StringBuilder();
            output.AppendLine(Util.AutoGeneratedMessage);
            output.AppendLine();
            output.AppendLine($"export enum {type.Name} {{");

            foreach (int value in Enum.GetValues(type))
            {
                var name = Enum.GetName(type, value);
                output.AppendLine($"    {name} = {value},");
            }

            output.AppendLine($"}}");

            var filePath = Path.Combine(outputPath, $"{type.Name}.ts");
            Util.ConvertLineEndings(output);
            File.WriteAllText(filePath, output.ToString());
        }
    }
}
