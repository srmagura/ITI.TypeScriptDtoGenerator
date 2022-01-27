using ITI.TypeScriptDtoGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MapDtoTests
    {
        private record Identity
        {
            public Guid Guid { get; }
        }

        private class GenericList<TRow>
        {
            public List<TRow> Rows { get; set; } = new();

            public int FullLengthBeforeFiltering { get; set; }
        }

        private class PairOfGenericLists<TRow1, TRow2>
        {
            public GenericList<TRow1> PageOne { get; set; } = new();
            public GenericList<TRow2> PageTwo { get; set; } = new();
        }

        private class ExampleObject
        {
            public PairOfGenericLists<Identity, string> Pair { get; set; } = new();
        }

        [TestMethod]
        public void ItIgnoresCompilerGeneratedProperties()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(Identity), new(), new DtoGenerationConfig());

            Assert.IsTrue(dtoString.Contains("guid: string"));
            Assert.IsFalse(dtoString.Contains("equalityContract"));
        }

        [TestMethod]
        public void CanGenerateGenericDtos()
        {
            var listString = DtoGenerator.GenerateDtoToString(typeof(GenericList<>), new(), new DtoGenerationConfig());
            Console.WriteLine(listString);

            Assert.IsTrue(listString.Contains("export interface GenericList<TRow> {"));
            Assert.IsTrue(listString.Contains("rows: TRow[]"));

            var pairString = DtoGenerator.GenerateDtoToString(typeof(PairOfGenericLists<,>), new() { typeof(PairOfGenericLists<,>), typeof(GenericList<>) }, new DtoGenerationConfig());
            Console.WriteLine(pairString);

            Assert.IsTrue(pairString.Contains("import { GenericList } from './GenericList'"));
            Assert.IsTrue(pairString.Contains("export interface PairOfGenericLists<TRow1,TRow2> {"));
            Assert.IsTrue(pairString.Contains("pageOne: GenericList<TRow1>"));
            Assert.IsTrue(pairString.Contains("pageTwo: GenericList<TRow2>"));
        }

        [TestMethod]
        public void CanGenerateGenericDtoUsages()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(ExampleObject), new() { typeof(Identity) }, new DtoGenerationConfig());
            Console.WriteLine(dtoString);

            Assert.IsTrue(dtoString.Contains("pair: PairOfGenericLists<Identity, string>"));
            Assert.IsTrue(dtoString.Contains("import { Identity } from './Identity'"));
        }

        private class GeoRectangle
        {
            public double NELat { get; set; }
            public double NELng { get; set; }
            public double SWLat { get; set; }
            public double SWLng { get; set; }
        }

        [TestMethod]
        public void ConsecutiveInitialCapitalsGetCamelizedToLower()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(GeoRectangle), new(), new DtoGenerationConfig());
            Console.WriteLine(dtoString);

            Assert.IsTrue(dtoString.Contains("neLat: number"));
        }

        private class ThingWithIdentity
        {
            public string? Name { get; set; }
            public Identity? Id { get; set; }
        }

        [TestMethod]
        public void ReferenceToOtherTypeGeneratesImport()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(ThingWithIdentity), new() { typeof(Identity) }, new DtoGenerationConfig());
            Console.WriteLine(dtoString);

            Assert.IsTrue(dtoString.Contains("import { Identity } from './Identity'"));
        }

        [TestMethod]
        public void TypeNameConst()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(ThingWithIdentity), new() { typeof(Identity) }, new DtoGenerationConfig
            {
                ExportTypeNameAsConstant = t => t == typeof(ThingWithIdentity)
            });
            Console.WriteLine(dtoString);

            Assert.IsTrue(dtoString.Contains("export const ThingWithIdentityTypeName = 'ThingWithIdentity'"));
        }

#nullable disable
        private class ThingWithReferences
        {
            public string Name { get; set; }
            public Identity Id { get; set; }
        }
#nullable enable

        [TestMethod]
        public void NullHandlingOptions()
        {
            {
                var dtoString = DtoGenerator.GenerateDtoToString(typeof(ThingWithReferences), new() { typeof(Identity) }, new DtoGenerationConfig
                {
                    NullHandling = DtoGenerationNullHandling.TreatUnknownAsNonNullable,
                });
                Console.WriteLine(dtoString);

                Assert.IsTrue(dtoString.Contains("name: string\n"));
            }
            {
                var dtoString = DtoGenerator.GenerateDtoToString(typeof(ThingWithReferences), new() { typeof(Identity) }, new DtoGenerationConfig
                {
                    NullHandling = DtoGenerationNullHandling.TreatUnknownAsNullable,
                });
                Console.WriteLine(dtoString);

                Assert.IsTrue(dtoString.Contains("name: string | null | undefined\n"));
            }
            {
                var dtoString = DtoGenerator.GenerateDtoToString(typeof(ThingWithReferences), new() { typeof(Identity) }, new DtoGenerationConfig
                {
                    NullHandling = DtoGenerationNullHandling.TreatAllReferenceTypesAsNullable,
                });
                Console.WriteLine(dtoString);

                Assert.IsTrue(dtoString.Contains("name: string | null | undefined\n"));
            }
        }
    }
}
