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
    public class DataSourceTests
    {
        [TestMethod]
        public void ParseWorks()
        {
            var source = new DataSource("cpu:GAUGE:0:U");
            Assert.AreEqual("cpu", source.Name);
            Assert.AreEqual(1, source.Type);
            Assert.AreEqual(0, source.Min);
            Assert.AreEqual(double.NaN, source.Max);
        }
    }
}
