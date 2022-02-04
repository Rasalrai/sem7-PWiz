using System;
using System.Collections.Generic;
using System.Reflection;
namespace BLC
{
    public sealed class BLC
    {
        Interfaces.IDAO dao;

        public static BLC blc = null;

        public BLC(string libraryName)
        {
            Assembly a = Assembly.UnsafeLoadFrom(libraryName);

            Type typeToCreate = null;
            Type IDAOType = typeof(Interfaces.IDAO);

            foreach (var t in a.GetTypes())
            {
                //IDAOType.IsAssignableFrom(T)
                if (t.IsAssignableTo(IDAOType))
                {
                    typeToCreate = t;
                    break;
                }
            }

            if (typeToCreate != null)
            {
                dao = (Interfaces.IDAO)Activator.CreateInstance(typeToCreate);
            }
        }

        public static BLC GetBLC()
        {
            if (blc == null && !string.IsNullOrEmpty(LibName))
            {
                blc = new BLC(LibName);
            }
            return blc;
        }

        public static string LibName { get; set; }

        public IEnumerable<Interfaces.IFood> GetFood()
        {
            return dao.GetAllFood();
        }

        public Interfaces.IFood CreateNewItem()
        {
            return dao.CreateNewItem();
        }

        public void SaveItem(Interfaces.IFood f)
        {
            dao.SaveItem(f);
        }

        public void RemoveItem(Interfaces.IFood f)
        {
            dao.RemoveItem(f);
        }

        public IEnumerable<Interfaces.IProducer> GetProducers()
        {
            return dao.GetAllProducers();
        }
    }
}
