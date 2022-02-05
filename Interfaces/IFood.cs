namespace Interfaces
{
    public interface IFood
    {
        int ID { get; set; }
        string Name { get; set; }
        IProducer Producer { get; set; }
        string ExpiryDate { get; set; }
        Core.Storage Storage { get; set; }
    }
}
