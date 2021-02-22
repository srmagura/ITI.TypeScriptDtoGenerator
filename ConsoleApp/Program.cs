using Dto;
using ITI.TypeScriptDtoGenerator;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
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

            foreach (var dto in dtos)
            {
                TypeScriptDtoGenerator.GenerateDto(dto, outputPath);
            }
            Console.WriteLine("Generated TypeScript DTOs.");

            TypeScriptDtoGenerator.GenerateIndex(outputPath);
            Console.WriteLine("Generated TypeScript DTO index.");
        }
    }
}
