using CollectdClient.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollectdClient.Tests
{
    [TestClass]
    public class UdpDispatchTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var dispatcher = new UdpDispatcher("172.16.20.126", 25826);
            var vl = new ValueList.Builder();
            vl.AddValue(10) //used
              .AddValue(100) //free
              .Plugin("df")
              .PluginInstance("0")
              .Type("df")
              .Interval(10);

            dispatcher.Dispatch(vl.Build());
        }
    }
}
