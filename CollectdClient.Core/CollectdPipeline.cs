using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    public class CollectdPipeline
    {
        public IPluginRepository Repository { get; set; }

        private readonly BroadcastBlock<ValueList> broadcast = new BroadcastBlock<ValueList>(x => x);

        public CollectdPipeline()
        {
        }

        public void RegisterWriter(IWriteInterface writer)
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

        public void Post(ValueList vl)
        {
            broadcast.Post(vl);
        }
    }
}
