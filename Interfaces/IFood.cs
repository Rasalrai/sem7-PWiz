using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IFood
    {
        int ID { get; set; }
        string Name { get; set; }
        IProducer Producer { get; set; }
        DateTime ExpiryDate { get; set; }
        Core.Storage Storage { get; set; }
    }
}
