namespace CollectdClient.Core.Plugins
{
    public interface IPluginMetadata
    {
        string Name { get; }
        int Interval { get; }
    }
}
