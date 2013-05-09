using CollectdClient.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectd.Riemann
{
    [Plugin("Riemann")]
    public class WriteRiemannPlugin : IWriteInterface
    {
        public int BatchSize
        {
            get { return 1; }
        }

        public bool WriteBatch(CollectdClient.Core.ValueList[] vl)
        {
            throw new NotImplementedException();
        }

        public bool Write(CollectdClient.Core.ValueList vl)
        {
            throw new NotImplementedException();
        }

        public void Register(PluginContext context)
        {
            throw new NotImplementedException();
        }
    }
}
