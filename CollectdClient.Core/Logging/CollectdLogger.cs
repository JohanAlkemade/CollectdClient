using System;
using Common.Logging;
using Common.Logging.Factory;

namespace CollectdClient.Core.Logging
{
    public class CollectdLogger : AbstractLogger
    {
        private string loggerName;
        private LogLevel logLevel;

        public CollectdLogger(string loggerName, LogLevel logLevel)
        {
            this.loggerName = loggerName;
            this.logLevel = logLevel;
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            if (exception != null)
            {

            }
            else
            {
                
            }
        }

        public override bool IsTraceEnabled
        {
            get { return logLevel <= LogLevel.Trace; }
        }

        public override bool IsDebugEnabled
        {
            get { return logLevel <= LogLevel.Debug; }
        }

        public override bool IsErrorEnabled
        {
            get { return logLevel <= LogLevel.Error; }
        }

        public override bool IsFatalEnabled
        {
            get { return logLevel <= LogLevel.Fatal; }
        }

        public override bool IsInfoEnabled
        {
            get { return logLevel <= LogLevel.Info ; }
        }

        public override bool IsWarnEnabled
        {
            get { return logLevel <= LogLevel.Warn; }
        }
    }
}
