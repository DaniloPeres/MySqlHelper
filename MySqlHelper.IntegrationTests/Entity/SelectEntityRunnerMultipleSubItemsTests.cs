
using MySqlHelper.Attributes;
using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class SelectEntityRunnerMultipleSubItemsTests
    {
        private EntityFactory entityFactory;
        private List<Customer> customers;

        [SetUp]
        public void SetUp()
        {
            Helper.DataBase.CleanTable<Customer>();
            Helper.DataBase.CleanTable<Order>();
            var config = new ConfigurationSettings();
            entityFactory = new EntityFactory(config.ConnectionString);
            SetDefaultRegisters();
            InsertDataForTests();
        }

        [Test]
        public void SelectAllRegistersWithSubItems()
        {
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Customer>()
                .WithSubItems(
                    TableAttribute.GetTableName<Customer>(),
                    TableAttribute.GetTableName<Order>(),
                    (ColumnAttribute.GetColumnName<Customer>(nameof(Customer.Id)), ColumnAttribute.GetColumnName<Order>(nameof(Order.CustomerId))));
            var customers = selectBuilder.Execute().ToList();


            // TODO
        }


        private void SetDefaultRegisters()
        {
            customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Publisher 1",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            Id = 1,
                            CustomerId = 1,
                            Date = DateTime.Parse("2020-01-01 00:00:00")
                        },
                        new Order
                        {
                            Id = 2,
                            CustomerId = 1,
                            Date = DateTime.Parse("2020-01-01 00:02:00")
                        }
                    }
                },
                new Customer
                {
                    Id = 2,
                    Name = "Publisher 2",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            Id = 3,
                            CustomerId = 2,
                            Date = DateTime.Parse("2020-01-02 10:00:00")
                        },
                        new Order
                        {
                            Id = 4,
                            CustomerId = 2,
                            Date = DateTime.Parse("2020-01-02 10:02:00")
                        },
                        new Order
                        {
                            Id = 5,
                            CustomerId = 2,
                            Date = DateTime.Parse("2020-01-02 10:02:00")
                        }
                    }
                }
            };
        }

        private void InsertDataForTests()
        {
            entityFactory.InsertMultiples(customers);

            var orders = new List<Order>();
            customers.ForEach(x => orders.AddRange(x.Orders));
            entityFactory.InsertMultiples(orders);
        }
    }
}
