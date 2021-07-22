# ITI.TypeScriptDtoGenerator

Converts C# classes and enums into TypeScript interfaces and enums. Based on reflection.

Example usage:

```
internal class Program
{
    internal static void Main()
    {
        var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TypeScript");

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        var enums = new List<Type>
        {
            typeof(ServiceType)
        };

        var dtos = new List<Type>
        {
            typeof(CustomerOrderDto),
            typeof(VendorOrderDto),
            typeof(UserDto),
            typeof(CustomerUserDto),
        };

        foreach (var @enum in enums)
        {
            TypeScriptDtoGenerator.GenerateEnum(@enum, outputPath);
        }
        Console.WriteLine("Generated TypeScript enums.");

        var imports = "import { EmailAddressDto } from './HandwrittenDtos'\n";

        TypeScriptDtoGenerator.GenerateDtos(dtos, enums, imports, outputPath);
        Console.WriteLine("Generated TypeScript DTOs.");

        TypeScriptDtoGenerator.GenerateIndex(outputPath);
        Console.WriteLine("Generated TypeScript DTO index.");
    }
}
```