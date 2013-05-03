
using System.ComponentModel.Composition;

namespace CollectdClient.Core.Plugins
{    
    public interface IPlugin
    {
        void Register(PluginContext context);
    }
}
