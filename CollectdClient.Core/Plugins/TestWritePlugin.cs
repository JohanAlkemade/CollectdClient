using System;

namespace CollectdClient.Core.Plugins
{
    [Plugin("testwrite")]
    public class TestWritePlugin : IWriteInterface
    {
        public void Register(PluginContext context)
        {

        }
        
        public bool Write(ValueList vl)
        {
            Console.WriteLine(vl.ToString());
            return false;
        }

        public int BatchSize
        {
            get { return 1; }
        }

        public bool WriteBatch(ValueList[] vl)
        {
            Console.WriteLine("Got values batch");
            return true;
        }
    }
}
