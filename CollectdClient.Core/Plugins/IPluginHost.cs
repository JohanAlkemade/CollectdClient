using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    public interface IPluginHost
    {
        /// <summary>
        /// Send values to the pipeline
        /// </summary>
        /// <param name="vl"></param>
        void DispatchValues(ValueList vl);
    }
}
