using System.Collections.Generic;
using System.Linq;
using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    public static class PluginRepositoryExtensions
    {
        public static IEnumerable<IReadInterface> GetReadPlugins(this IPluginRepository repository)
        {
            return repository.GetCurrentPlugins().OfType<IReadInterface>();
        }

        public static IEnumerable<IWriteInterface> GetWritePlugins(this IPluginRepository repository)
        {
            return repository.GetCurrentPlugins().OfType<IWriteInterface>();
        }

        public static IEnumerable<IInitInterface> GetInitPlugins(this IPluginRepository repository)
        {
            return repository.GetCurrentPlugins().OfType<IInitInterface>();
        }

        public static IEnumerable<IShutdownInterface> GetShutdownPlugins(this IPluginRepository repository)
        {
            return repository.GetCurrentPlugins().OfType<IShutdownInterface>();
        }

        public static IEnumerable<IConfigInterface> GetConfigPlugins(this IPluginRepository repository)
        {
            return repository.GetCurrentPlugins().OfType<IConfigInterface>();
        }
    }
}
