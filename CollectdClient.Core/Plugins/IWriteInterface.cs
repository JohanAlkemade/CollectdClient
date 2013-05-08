using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    public interface IWriteInterface : IPlugin
    {
        Task<bool> Write(ValueList[] vl);
    }
}
