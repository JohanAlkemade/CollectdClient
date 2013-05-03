namespace CollectdClient.Core
{
    public interface IDispatcher
    {
        void Dispatch(ValueList values);
    }
}
