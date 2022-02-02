using Core;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DateTime ExpiryDate { get; set; }
        public Storage Storage { get; set; }
    }
}
