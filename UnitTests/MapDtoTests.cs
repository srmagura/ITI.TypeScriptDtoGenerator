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
            public List<TRow> Rows { get; set; }

            public int FullLengthBeforeFiltering { get; set; }
        }

        private class PairOfGenericLists<TRow1, TRow2>
        {
            public GenericList<TRow1> PageOne { get; set; }
            public GenericList<TRow2> PageTwo { get; set; }
        }

        private class ExampleObject
        {
            public PairOfGenericLists<Identity, string> Pair { get; set; }
        }

        [TestMethod]
        public void ItIgnoresCompilerGeneratedProperties()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(Identity), new(), null);

            Assert.IsTrue(dtoString.Contains("guid: string"));
            Assert.IsFalse(dtoString.Contains("equalityContract"));
        }

        [TestMethod]
        public void CanGenerateGenericDtos()
        {
            var listString = DtoGenerator.GenerateDtoToString(typeof(GenericList<>), new(), null);
            Console.WriteLine(listString);

            Assert.IsTrue(listString.Contains("export interface GenericList<TRow> {"));
            Assert.IsTrue(listString.Contains("rows: TRow[]"));

            var pairString = DtoGenerator.GenerateDtoToString(typeof(PairOfGenericLists<,>), new(), null);
            Console.WriteLine(pairString);

            Assert.IsTrue(pairString.Contains("export interface PairOfGenericLists<TRow1,TRow2> {"));
            Assert.IsTrue(pairString.Contains("pageOne: GenericList<TRow1>"));
            Assert.IsTrue(pairString.Contains("pageTwo: GenericList<TRow2>"));
        }

        [TestMethod]
        public void CanGenerateGenericDtoUsages()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(ExampleObject), new(), null);
            Console.WriteLine(dtoString);

            Assert.IsTrue(dtoString.Contains("pair: PairOfGenericLists<Identity, string>"));
        }
    }
}
