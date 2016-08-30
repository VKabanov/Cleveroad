using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;

namespace Cleveroad.Models
{
    public class OrdersContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Entity<Order>().
                          Property(p => p.OrderDate)
                          .HasColumnType("datetime2")
                          .HasPrecision(0)
                          .IsRequired();
        }        
    }

    interface IRepository<T> : IDisposable
    where T : class
    {
        IEnumerable<T> GetAll(bool? DetectChanges);
        T GetFirst(int id, bool? DetectChanges);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Delete(T item);
        void SaveChanges();
    }

    public class OrdersRepository : IRepository<Order>
    {
        OrdersContext db;
        private bool disposed = false;

        public OrdersRepository()
        {
            db = new OrdersContext();
        }
        public void Create(Order order)
        {
            db.Orders.Add(order);
        }
        public void Update(Order order)
        {
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
                db.Orders.Remove(order);
        }
        public void Delete(Order order)
        {
            if (order != null)
            db.Entry(order).State = System.Data.Entity.EntityState.Deleted;
        }
    
        public void SaveChanges()
        {
            db.SaveChanges();
        }
        public Order GetFirst(int id, bool? DetectChanges)
        {
            bool AutoDetectChangesFlag = db.Configuration.AutoDetectChangesEnabled;

            if (DetectChanges==null)   db.Configuration.AutoDetectChangesEnabled = true;
            else db.Configuration.AutoDetectChangesEnabled = false;

            Order result = db.Orders.First(o => o.Id == id);

            db.Configuration.AutoDetectChangesEnabled = AutoDetectChangesFlag;
            return result;
        }
        public IEnumerable<Order> GetAll(bool? DetectChanges)
        {
            bool AutoDetectChangesFlag = db.Configuration.AutoDetectChangesEnabled;

            if (DetectChanges == null) db.Configuration.AutoDetectChangesEnabled = true;
            else db.Configuration.AutoDetectChangesEnabled = false;

            IEnumerable<Order>result=db.Orders.ToList();
            db.Configuration.AutoDetectChangesEnabled = AutoDetectChangesFlag;
            return result;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}