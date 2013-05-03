using Common.Logging;
using Common.Logging.Factory;

namespace CollectdClient.Core.Logging
{
    public class CollectdLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private LogLevel logLevel;

        protected override ILog CreateLogger(string name)
        {
            return new CollectdLogger(name, logLevel);
        }
    }
}
