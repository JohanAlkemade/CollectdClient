using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    public interface  IReadInterface : IPlugin
    {
        Task<bool> Read();
    }
}