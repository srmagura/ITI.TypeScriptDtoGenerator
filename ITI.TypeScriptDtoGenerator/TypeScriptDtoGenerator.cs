namespace ITI.TypeScriptDtoGenerator;

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
        DtoGenerationConfig config
    )
    {
        DtoGenerator.GenerateDtos(dtoTypes, couldImportTypes, config);
    }

    public static void GenerateDtos(
        List<Type> dtoTypes,
        List<Type> couldImportTypes,
        string imports,
        string outputPath,
        Func<Type, bool> exportTypeNameAsConstant
    ) => GenerateDtos(dtoTypes, couldImportTypes, new DtoGenerationConfig
        {
            Imports = imports,
            OutputPath = outputPath,
            ExportTypeNameAsConstant = exportTypeNameAsConstant,
        });

    public static void GenerateDtos(
        List<Type> dtoTypes,
        List<Type> couldImportTypes,
        string imports,
        string outputPath
    ) => GenerateDtos(dtoTypes, couldImportTypes, imports, outputPath, t => false);
}
