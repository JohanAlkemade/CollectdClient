using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    public interface  IBatchedWriteInterface : IWriteInterface
    {
        int BatchSize { get; }
        Task<bool> WriteBatch(ValueList[] vl);
    }

    public interface IWriteInterface : IPlugin
    {
        Task<bool> Write(ValueList vl);
    }
}
