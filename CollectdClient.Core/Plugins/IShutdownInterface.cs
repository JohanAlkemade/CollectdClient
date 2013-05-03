
namespace CollectdClient.Core.Plugins
{
    public interface IShutdownInterface : IPlugin
    {
        bool Shutdown();
    }
}
