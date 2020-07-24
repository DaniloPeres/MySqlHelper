using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using NUnit.Framework;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class ReplaceEntityRunnerTests
    {
        [Test]
        public void Replace_InsertAndUpdateRegisterByModel()
        {
            // Arrange
            var config = new ConfigurationSettings();
            var entityFactory = new EntityFactory(config.ConnectionString);
            var book = new Book
            {
                Id = 1,
                Title = "Book Test new with replace",
                Price = 1.39m
            };
            // delete to clean the test
            entityFactory.Delete(book);

            entityFactory.Replace(book);

            // Check auto generated id
            Assert.AreNotEqual(0, book.Id);

            var selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnNameWithQuotes<Book>(nameof(Book.Id)), book.Id));


            var books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(Helper.Comparer.IsSameBook(book, books.First()));

            book.Price = 3.29m;
            book.Title = "Book Test update with replace";

            // Act
            entityFactory.Replace(book);

            // Assert
            selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnNameWithQuotes<Book>(nameof(Book.Id)), book.Id));

            books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(Helper.Comparer.IsSameBook(book, books.First()));
        }
    }
}
