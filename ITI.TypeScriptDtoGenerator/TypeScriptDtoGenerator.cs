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

        public static void GenerateDtos(
            List<Type> dtoTypes,
            List<Type> couldImportTypes,
            string imports,
            string outputPath
        )
        {
            DtoGenerator.GenerateDtos(dtoTypes, couldImportTypes, imports, outputPath);
        }
    }
}
