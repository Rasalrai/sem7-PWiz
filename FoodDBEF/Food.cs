using Core;
using Interfaces;

namespace FoodDBEF
{
    public class Food : Interfaces.IFood
    {
        public int ID { get; set; }
        public string Name { get; set; }
        IProducer IFood.Producer
        {
            get => Producer;
            set
            {
                Producer = value as Producer;
            }
        }
        public Producer Producer { get; set; }

        public string ExpiryDate { get; set; }
        public Storage Storage { get; set; }
    }
}
