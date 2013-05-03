using System;

namespace CollectdClient.Core
{
    public abstract class Sender : IDispatcher
    {

        public void Dispatch(ValueList values)
        {
            Write(values);
        }

        protected abstract void Write(PluginData data);
    }
}
