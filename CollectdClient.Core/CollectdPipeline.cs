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
            //if the plugin uses a batched approach, add a batchblock between 
            //the broadcast and the action blocks
            if (writer.BatchSize > 1)
            {
                var bufferBlock = new BatchBlock<ValueList>(writer.BatchSize);
                bufferBlock.LinkTo(new ActionBlock<ValueList[]>(vls => writer.WriteBatch(vls)), new DataflowLinkOptions() { PropagateCompletion = true });

                broadcast.LinkTo(bufferBlock, new DataflowLinkOptions() { PropagateCompletion = true });
            }
            else
            {
                broadcast.LinkTo(new ActionBlock<ValueList>(vl => writer.Write(vl)), new DataflowLinkOptions() { PropagateCompletion = true });
            }
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
