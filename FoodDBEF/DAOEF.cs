using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDBEF
{
    public class DAOEF : DbContext, Interfaces.IDAO
    {
        public DbSet<Producer> Producers { get; set; }

        public DbSet<Food> Foods { get; set; }

        IFood IDAO.CreateNewItem()
        {
            IFood f = new Food();
            f.Storage = Core.Storage.Fridge;
            f.ExpiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            return f;
        }

        IEnumerable<IFood> IDAO.GetAllFood()
        {
            return Foods.ToList();
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

        #region Producers

        IEnumerable<IProducer> IDAO.GetAllProducers()
        {
            return Producers.ToList();
        }

        public IProducer CreateNewProducer()
        {
            return new Producer();
        }
        public void SaveProducer(IProducer producer)
        {
            if (producer != null)
            {
                Producers.Add(producer as Producer);
            }
            SaveChanges();
        }

        public void RemoveProducer(IProducer producer)
        {
            Producers.Remove(producer as Producer);
            SaveChanges();
        }

        #endregion
    }
}
