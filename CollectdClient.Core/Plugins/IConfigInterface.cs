using Newtonsoft.Json.Linq;

namespace CollectdClient.Core.Plugins
{
    public interface IConfigInterface : IPlugin
    {
        void Config(IConfigProvider provider);
    }
}
