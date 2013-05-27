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
    public sealed class CollectdServiceInstallerProcess : ServiceProcessInstaller
    {
        public CollectdServiceInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }
    }
}
