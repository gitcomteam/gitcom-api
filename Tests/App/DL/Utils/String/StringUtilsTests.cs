using App.DL.Utils.String;
using NUnit.Framework;
using Tests.Testing;

namespace Tests.App.DL.Utils.String {
    [TestFixture]
    public class StringUtilsTests : BaseTestFixture {
        [Test]
        public void Implode_DataCorrect_Ok() {
            var strings = new [] {"str1", "str2"};
            Assert.AreEqual("str1,str2", StringUtils.Implode(strings, ","));
        }
        
        [Test]
        public void Implode_DataCorrect_WithQuotes() {
            var strings = new [] {"str1", "str2"};
            Assert.AreEqual("'str1','str2'", StringUtils.Implode(strings, ",", true));
        }
    }
}