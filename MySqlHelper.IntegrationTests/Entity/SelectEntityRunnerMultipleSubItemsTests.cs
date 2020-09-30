
using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using MySqlHelper.IntegrationTests.Helper;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class SelectEntityRunnerMultipleSubItemsTests
    {
        private EntityFactory entityFactory;
        private List<Customer> customersInternal;

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
            // Arrange
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Customer>()
                .WithSubItems(typeof(Order));

            // Act
            var customers = selectBuilder.Execute().ToList();

            // Assert
            Assert.AreEqual(customersInternal.Count, customers.Count);
            for (var i = 0; i < customers.Count; i++)
            {
                Assert.True(Comparer.IsSameCustomer(customersInternal[i], customers[i]));
            }
        }


        private void SetDefaultRegisters()
        {
            customersInternal = new List<Customer>
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
                            TotalPrice = 11.11m
                        },
                        new Order
                        {
                            Id = 2,
                            CustomerId = 1,
                            TotalPrice = 11.22m
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
                            TotalPrice = 22.11m
                        },
                        new Order
                        {
                            Id = 4,
                            CustomerId = 2,
                            TotalPrice = 22.22m
                        },
                        new Order
                        {
                            Id = 5,
                            CustomerId = 2,
                            TotalPrice = 22.33m
                        }
                    }
                }
            };
        }

        private void InsertDataForTests()
        {
            entityFactory.InsertMultiples(customersInternal, false);

            var orders = new List<Order>();
            customersInternal.ForEach(x => orders.AddRange(x.Orders));
            entityFactory.InsertMultiples(orders, false);
        }
    }
}
