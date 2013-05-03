using System;

namespace CollectdClient.Core.Plugins
{
    public class PluginContext
    {

        public void DispatchValues(ValueList list)
        {
            var dispatcher = new UdpDispatcher("172.16.20.126", 25826);
            dispatcher.Dispatch(list);
        }
    }
}
