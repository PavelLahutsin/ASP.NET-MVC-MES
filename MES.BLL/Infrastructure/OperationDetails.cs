namespace MES.BLL.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Accessory = prop;
        }
        public bool Succedeed { get; }
        public string Message { get; }
        public string Accessory { get; }
    }
}
