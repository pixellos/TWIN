namespace StorageTypes.AppInterface.ClientHub
{
    public interface NodeConnectionInterface
    {
        void sendData(DataToDisplay data, object[] args = null);
        void Hello();
    }
}