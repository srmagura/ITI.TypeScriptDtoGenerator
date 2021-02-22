using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.TypeScriptDtoGenerator
{
    public static class TypeScriptDtoGenerator
    {
        public static void GenerateIndex(string directoryPath)
        {
            IndexGenerator.GenerateIndex(directoryPath);
        }

        public static void GenerateEnum(Type type, string outputPath)
        {
            EnumGenerator.GenerateEnum(type, outputPath);
        }

        public static void GenerateDto(Type type, string outputPath)
        {
            DtoGenerator.GenerateDto(type, outputPath);
        }
    }
}
