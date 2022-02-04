using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDBEF
{
    public class DAOEF : DbContext, Interfaces.IDAO
    {
        public DbSet<Producer> Producers { get; set; }

        public DbSet<Food> Foods { get; set; }

        IFood IDAO.CreateNewItem()
        {
            // TODO
            IFood f = new Food();
            //f.Producer = producers[0];
            f.Storage = Core.Storage.Fridge;
            f.ExpiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            return f;
        }

        IEnumerable<IFood> IDAO.GetAllFood()
        {
            return Foods.ToList();
        }

        IEnumerable<IProducer> IDAO.GetAllProducers()
        {
            return Producers.ToList();
        }

        public void SaveItem(IFood food)
        {
            if (food != null)
            {
                Foods.Add(food as Food);
            }
            SaveChanges();
        }

        public void RemoveItem(IFood food)
        {
            Foods.Remove(food as Food);
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string dbPath = @"Fridge.db";
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
