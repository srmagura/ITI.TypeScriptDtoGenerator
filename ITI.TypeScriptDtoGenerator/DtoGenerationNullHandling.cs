namespace ITI.TypeScriptDtoGenerator;

public enum DtoGenerationNullHandling
{
    TreatUnknownAsNonNullable = 0,
    TreatUnknownAsNullable = 1,
    TreatAllReferenceTypesAsNullable = 2,
}
