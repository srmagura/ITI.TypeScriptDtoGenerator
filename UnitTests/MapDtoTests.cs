using ITI.TypeScriptDtoGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
