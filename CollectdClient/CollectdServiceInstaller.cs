using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient
{
    [RunInstaller(true)]
    public sealed class CollectdServiceInstaller : ServiceInstaller
    {
        public CollectdServiceInstaller()
        {
            this.ServiceName = "Collectd";
            this.StartType = ServiceStartMode.Automatic;
            this.DisplayName = "Collectd";
        }
    }
}
