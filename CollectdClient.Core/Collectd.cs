

namespace CollectdClient.Core
{
    public class Collectd
    {
        public static CollectdPipeline Pipeline { get; set; }

        static Collectd()
        {
            Pipeline = new CollectdPipeline();
        }

        public static void DispatchValues(ValueList vl)
        {
            Pipeline.Post(vl);
        }
    }
}
