using System.Collections.Generic;

namespace CollectdClient.Core.Plugins
{
    public interface IWriteInterface : IPlugin
    {
        int BatchSize { get; }
        bool WriteBatch(ValueList[] vl);
        bool Write(ValueList vl);
    }
}
