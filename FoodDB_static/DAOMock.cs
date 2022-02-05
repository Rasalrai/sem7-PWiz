using Interfaces;
using System;
using System.Collections.Generic;

namespace FoodDB_static
{
    public class DAOMock : Interfaces.IDAO
    {
        private List<IProducer> producers;
        private List<IFood> food;

        public DAOMock()
        {
            producers = new List<IProducer>()
            {
                new Producer() {ID=1, Name="Alpro", Residence=Core.Region.Europe},
                new Producer() {ID=2, Name="Violife", Residence=Core.Region.Europe},
                new Producer() {ID=3, Name="Polsoja", Residence=Core.Region.Poland},
                new Producer() {ID=4, Name="Sante", Residence=Core.Region.Poland}
            };

            food = new List<IFood>()
            {
                new Food() {ID=1, Producer=producers[0], Name="Mleko owsiane", Storage=Core.Storage.Fridge, ExpiryDate="2022-02-11"},
                new Food() {ID=2, Producer=producers[0], Name="Jogurt sojowy", Storage=Core.Storage.Fridge, ExpiryDate="2022-02-16"},
                new Food() {ID=3, Producer=producers[2], Name="Tofu wędzone", Storage=Core.Storage.Freezer, ExpiryDate="2022-03-01"},
                new Food() {ID=4, Producer=producers[3], Name="Kotlety sojowe", Storage=Core.Storage.Cupboard, ExpiryDate="2023-05-22"}
            };
        }

        public IFood CreateNewItem()
        {
            IFood f = new Food();
            f.Producer = producers[0];
            f.ExpiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            f.Storage = Core.Storage.Cupboard;
            return f;
        }

        public IEnumerable<IFood> GetAllFood()
        {
            return food;
        }

        public void SaveItem(IFood f)
        {
            if (food != null)
            {
                food.Add(f);
            }
        }

        public void RemoveItem(IFood f)
        {
            food.Remove(f);
        }

        #region Producers

        public IEnumerable<IProducer> GetAllProducers()
        {
            return producers;
        }

        public IProducer CreateNewProducer()
        {
            return new Producer();
        }

        public void SaveProducer(IProducer producer)
        {
            if (producer != null)
            {
                producers.Add(producer as Producer);
            }
        }

        public void RemoveProducer(IProducer producer)
        {
            producers.Remove(producer as Producer);
        }

        #endregion
    }
}
