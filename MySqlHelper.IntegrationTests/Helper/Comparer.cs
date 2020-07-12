using MySqlHelper.IntegrationTests.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.IntegrationTests.Helper
{
    public static class Comparer
    {
        public static bool IsSameBook(Book book1, Book book2)
        {
            return
                book1.Id.Equals(book2.Id)
                && book1.Title.Equals(book2.Title)
                && book1.Price.Equals(book2.Price)
                && book1.PublisherId.Equals(book2.PublisherId);
        }

        public static bool IsSamePublisher(Publisher publisher1, Publisher publisher2)
        {
            return
                publisher1.Id.Equals(publisher2.Id)
                && publisher1.Name.Equals(publisher2.Name);
        }

        public static bool IsSameCustomer(Customer customer1, Customer customer2)
        {
            if (!customer1.Id.Equals(customer2.Id)
                || !customer1.Name.Equals(customer2.Name))
                return false;

            if (customer1.Orders.Count != customer2.Orders.Count)
                return false;

            for (var i = 0; i < customer1.Orders.Count; i++)
            {
                if (!IsSameOrder(customer1.Orders[i], customer2.Orders[i]))
                    return false;
            }

            return true;
        }

        public static bool IsSameOrder(Order order1, Order order2)
        {
            return order1.Id.Equals(order2.Id)
                   && order1.TotalPrice.Equals(order2.TotalPrice)
                   && order1.CustomerId.Equals(order2.CustomerId);
        }
    }
}
