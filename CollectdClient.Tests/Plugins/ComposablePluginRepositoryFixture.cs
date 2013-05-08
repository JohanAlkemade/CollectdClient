using System;
using System.Linq;
using CollectdClient.Core;
using CollectdClient.Core.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CollectdClient.Tests.Plugins
{
    [TestClass]
    public class ComposablePluginRepositoryFixture
    {
        private IPluginRepository repository;
        
        [TestInitialize]
        public void Init()
        {
            repository = new ComposablePluginRepository(new Lazy<IPlugin, IPluginMetadata>[]
            {
                new Lazy<IPlugin, IPluginMetadata>(() => new Mock<IPlugin>().Object, new PluginAttribute("test"))
            });
        }

        [TestMethod]
        public void GetCurrentPlugins_Returns_Only_Enabled_Plugins()
        {
            Assert.AreEqual(0, repository.GetCurrentPlugins().Count());

            repository.EnablePlugin("test");

            Assert.AreEqual(1, repository.GetCurrentPlugins().Count());
        }
    }
}
