using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CollectdClient.Core.Plugins;

namespace CollectdClient.Core
{
    public class Pipeline
    {
        private readonly BroadcastBlock<ValueList> broadcast = new BroadcastBlock<ValueList>(x => x);

        public Pipeline()
        {
        }

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
            broadcast.Post(vl);
        }
    }
}
