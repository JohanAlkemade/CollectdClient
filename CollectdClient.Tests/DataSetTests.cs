using CollectdClient.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Tests
{
    [TestClass]
    public class DataSetTests
    {
        [TestMethod]
        public void Parse_Honours_Spaces_And_Tabs()
        {
            string line = " test       value:GAUGE:0:U  ";
            var ds = DataSet.Parse(line);
            Assert.AreEqual("test", ds.Type);
            Assert.AreEqual(1, ds.DataSources.Count());
        }

        [TestMethod]
        public void Parse_Supports_Multi_DataSources()
        {
            string line = "test     value:GAUGE:0:U, test:COUNTER:12312:349857345";
            var ds = DataSet.Parse(line);
            Assert.AreEqual("test", ds.Type);
            Assert.AreEqual(2, ds.DataSources.Count());
        }
    }
}
