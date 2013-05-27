using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    [Export(typeof(IPluginHost))]
    public class DefaultPluginHost : IPluginHost
    {
        public DefaultPluginHost()
        {
        }

        public void DispatchValues(ValueList vl)
        {
            Pipeline.Current.Process(vl);
        }
    }
}
