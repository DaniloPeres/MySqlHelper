using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using NUnit.Framework;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class DeleteEntityRunnerTests
    {
        [Test]
        public void DeleteRegisterByModel()
        {
            // Arrange
            var config = new ConfigurationSettings();
            var entityFactory = new EntityFactory(config.ConnectionString);
            var book = new Book
            {
                Title = "Book Test delete",
                Price = 2.99m
            };
            entityFactory.Insert(book);

            // Check auto genereted id
            Assert.AreNotEqual(0, book.Id);

            var selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));


            var books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(Helper.Comparer.IsSameBook(book, books.First()));


            // Act
            entityFactory.Delete(book);

            // Assert
            selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));

            books = selectBuilder.Execute();
            Assert.AreEqual(0, books.Count);
        }
    }
}
