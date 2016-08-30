using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Cleveroad.Models
{
    public class OrdersDBInitializer : DropCreateDatabaseAlways<OrdersContext>
    {
        protected override void Seed(OrdersContext db)
        {
            db.Customers.Add(new Customer { Id = 1, Name = "John Doe", Address = "Key House, 18 Seaward Place, Centurion Business Park, Glasgow, G41 1HH, GB", DateOfBirth = new DateTime(1977, 04, 05) });
            db.Customers.Add(new Customer { Id = 2, Name = "Joahn Doe",  Address = "Key House, 18 Seaward Place, Centurion Business Park, Glasgow, G41 1HH, GB", DateOfBirth = new DateTime(1978, 12, 03) });
            db.Customers.Add(new Customer { Id = 3, Name = "Nancy Reagan", Address = "40 Presidential Drive Simi Valley, California 93065, USA", DateOfBirth = new DateTime(1911, 02, 06) });
            db.Customers.Add(new Customer { Id = 4, Name = "Ronald Reagan", Address = "40 Presidential Drive Simi Valley, California 93065, USA", DateOfBirth = new DateTime(1921, 07, 06) });

            db.Orders.Add(new Order { Id = 1, CustomerId=1, OrderDate = DateTime.Today, OrderData = "Salmon", OrderRoundedWeight = 12 });
            db.Orders.Add(new Order { Id = 3, CustomerId=1, OrderDate = new DateTime(2016, 02, 06), OrderData = "Pears", OrderRoundedWeight = 8 });
            db.Orders.Add(new Order { Id = 4, CustomerId=2, OrderDate = new DateTime(2016, 07, 06), OrderData = "Whiskey", OrderRoundedWeight = 2 });
            db.Orders.Add(new Order { Id = 6, CustomerId=2, OrderDate = new DateTime(2016, 07, 06), OrderData = "Vodka", OrderRoundedWeight = 2 });

            db.Orders.Add(new Order { Id = 7, CustomerId= 3, OrderDate = DateTime.Today, OrderData = "Chicken", OrderRoundedWeight = 4 });
            db.Orders.Add(new Order { Id = 8, CustomerId= 3, OrderDate = new DateTime(2016, 02, 06), OrderData = "Beef", OrderRoundedWeight = 5 });
            db.Orders.Add(new Order { Id = 9, CustomerId= 4, OrderDate = new DateTime(2016, 07, 06), OrderData = "Butter", OrderRoundedWeight = 1 });
            db.Orders.Add(new Order { Id = 10, CustomerId= 2, OrderDate = new DateTime(2016, 07, 06), OrderData = "Bread", OrderRoundedWeight = 2 });
            db.Orders.Add(new Order { Id = 11, CustomerId= 4, OrderDate = new DateTime(2016, 07, 06), OrderData = "Caviar", OrderRoundedWeight = 1 });
            db.Orders.Add(new Order { Id = 12, CustomerId= 4, OrderDate = new DateTime(2016, 07, 06), OrderData = "Apples", OrderRoundedWeight = 4 });
            db.Orders.Add(new Order { Id = 2, CustomerId = 2, OrderDate = new DateTime(2016, 01, 01), OrderData = "Potato", OrderRoundedWeight = 22 });
            db.Orders.Add(new Order { Id = 5, CustomerId = 1, OrderDate = new DateTime(2016, 02, 06), OrderData = "Carrot", OrderRoundedWeight = 5 });

            base.Seed(db);
        }
    }
}