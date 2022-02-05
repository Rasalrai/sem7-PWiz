namespace FoodDB_static
{
    public class Producer : Interfaces.IProducer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Core.Region Residence { get; set; }
    }
}
