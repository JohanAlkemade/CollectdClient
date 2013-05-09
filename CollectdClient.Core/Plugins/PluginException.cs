using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectdClient.Core.Plugins
{
    public class PluginException : Exception
    {
        private Exception e;

        public PluginException(string message)
            : base(message)
        {
        }

        public PluginException(Exception e)
            : base(e.Message, e)
        {
        }
    }
}
