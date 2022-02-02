using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDB1
{
    public class DAOMock : Interfaces.IDAO
    {
        private List<IProducer> producers;
        private List<IFood> food;

        public DAOMock()
        {
            producers = new List<IProducer>()
            {
                new Producer() {ID=1, Name="Alpro"},
                new Producer() {ID=2, Name="Violife"},
                new Producer() {ID=3, Name="Polsoja"},
                new Producer() {ID=4, Name="Sante"}
            };

            food = new List<IFood>()
            {
                new Food() {ID=1, Producer=producers[0], Name="Mleko owsiane", Storage=Core.Storage.Fridge, ExpiryDate=new DateTime(2022, 2, 11)},
                new Food() {ID=2, Producer=producers[0], Name="Jogurt sojowy", Storage=Core.Storage.Fridge, ExpiryDate=new DateTime(2022, 2, 16)},
                new Food() {ID=3, Producer=producers[2], Name="Tofu wędzone", Storage=Core.Storage.Freezer, ExpiryDate=new DateTime(2022, 3, 1)},
                new Food() {ID=4, Producer=producers[3], Name="Kotlety sojowe", Storage=Core.Storage.Cupboard, ExpiryDate=new DateTime(2023, 5, 1)}
            };


        }
        public IFood CreateNewItem()
        {
            IFood f = new Food();
            f.Producer = producers[0];
            f.ExpiryDate = DateTime.Today.AddDays(7);
            f.Storage = Core.Storage.Cupboard;
            return f;
        }

        public IEnumerable<IFood> GetAllFood()
        {
            return food;
        }

        public IEnumerable<IProducer> GetAllProducers()
        {
            return producers;
        }

        public void SaveItem(IFood f)
        {
            // todo validation
            food.Add(f);
        }
    }
}
