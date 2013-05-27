using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CollectdClient.Core.Plugins;
using System;
using NLog;

namespace CollectdClient.Core
{
    /*
     * The glue between readers and writers 
     */
    public class Pipeline
    {
        private readonly BroadcastBlock<ValueList> broadcast = new BroadcastBlock<ValueList>(x => x);
        private static Lazy<Pipeline> instance = new Lazy<Pipeline>(() => new Pipeline());
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool isInitialized;

        private Pipeline()
        {
        }

        public static Pipeline Current { get { return instance.Value; } }


        public void AddWriter(IWriteInterface writer)
        {
            var batchBlock = new BatchBlock<ValueList>(int.MaxValue);
            
            var timer = new Timer(state => batchBlock.TriggerBatch());
            timer.Change(0, 10000);

            broadcast.LinkTo(batchBlock);

            batchBlock.LinkTo(new ActionBlock<ValueList[]>(vl => writer.Write(vl)));
        }
        
        public void Complete()
        {
            broadcast.Complete();
            broadcast.Completion.Wait();
        }

        public void Process(ValueList vl)
        {
            logger.Debug("Process: time = {0}, interval = {1}; host = {2}; plugin = {3}; plugin_instance = {4}; type = {5}; type_instance = {6};", 
                vl.Time,
                vl.Interval, 
                vl.Host,
                vl.Plugin, 
                vl.PluginInstance, 
                vl.Type, 
                vl.TypeInstance);

            broadcast.Post(vl);
        }
    }
}
