

namespace CollectdClient.Core
{
    public class Collectd
    {
        public static Pipeline Pipeline { get; set; }

        static Collectd()
        {
            Pipeline = new Pipeline();
        }

        public static void DispatchValues(ValueList vl)
        {
            Pipeline.Process(vl);
        }
    }
}
