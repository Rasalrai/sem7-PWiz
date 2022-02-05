using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDAO
    {
        IEnumerable<IProducer> GetAllProducers();
        IEnumerable<IFood> GetAllFood();

        IFood CreateNewItem();

        void SaveItem(IFood food);
        void RemoveItem(IFood food);

        IProducer CreateNewProducer();
        void SaveProducer(IProducer producer);
        void RemoveProducer(IProducer producer);
    }
}
