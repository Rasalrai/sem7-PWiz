namespace Interfaces
{
    public interface IProducer
    {
        int ID { get; set; }
        string Name { get; set; }
        Core.Region Residence { get; set; }
    }
}
