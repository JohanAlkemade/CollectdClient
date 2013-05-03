using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    public interface IPluginRepository
    {
        bool Exists(string pluginName);
        IPlugin GetPlugin(string pluginName);

        IEnumerable<IPlugin> GetCurrentPlugins();
    }


    //startup phase:
    //for each assembly in the bin directory
    //  scan the assembly and register the plugin into the repostiory

    //config phase:
    //for each enabled plugin in the config
    //  load the plugin
    //  call the register function
}
