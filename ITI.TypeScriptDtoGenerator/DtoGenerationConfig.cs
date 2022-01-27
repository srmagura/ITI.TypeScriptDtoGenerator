namespace ITI.TypeScriptDtoGenerator;

public class DtoGenerationConfig
{
    public Func<Type, bool> ExportTypeNameAsConstant { get; set; } = t => false;
    public string? Imports { get; set; }
    public string OutputPath { get; set; } = ".";
}
