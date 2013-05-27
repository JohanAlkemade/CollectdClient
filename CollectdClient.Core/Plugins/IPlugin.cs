
using System.ComponentModel.Composition;

namespace CollectdClient.Core.Plugins
{    
    public interface IPlugin
    {
        bool Register(IPluginHost host);
    }
}
