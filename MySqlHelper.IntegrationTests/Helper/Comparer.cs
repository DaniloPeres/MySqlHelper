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
    }
}
