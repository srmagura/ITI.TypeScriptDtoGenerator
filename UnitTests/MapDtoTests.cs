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

        [TestMethod]
        public void ItIgnoresCompilerGeneratedProperties()
        {
            var dtoString = DtoGenerator.GenerateDtoToString(typeof(Identity), new(), null);

            Assert.IsTrue(dtoString.Contains("guid: string"));
            Assert.IsFalse(dtoString.Contains("equalityContract"));
        }
    }
}
