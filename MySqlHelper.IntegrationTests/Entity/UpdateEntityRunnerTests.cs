using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using NUnit.Framework;
using System.Linq;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class UpdateEntityRunnerTests
    {
        [Test]
        public void UpdateByModel()
        {
            // Arrange
            var config = new ConfigurationSettings();
            var entityFactory = new EntityFactory(config.ConnectionString);
            var book = new Book
            {
                Title = "Book Test new",
                Price = 1.99m
            };
            entityFactory.Insert(book);

            // Check auto genereted id
            Assert.AreNotEqual(0, book.Id);

            var selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));


            var books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(Helper.Comparer.IsSameBook(book, books.First()));

            book.Price = 3.99m;
            book.Title = "Book Test update";

            // Act
            entityFactory.Update(book);

            // Assert
            selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));

            books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(Helper.Comparer.IsSameBook(book, books.First()));
        }

        [Test]
        public void UpdateSpecificFieldsByModel()
        {
            // Arrange
            var config = new ConfigurationSettings();
            var entityFactory = new EntityFactory(config.ConnectionString);
            var book = new Book
            {
                Title = "Book Test fields",
                Price = 1.19m
            };
            entityFactory.Insert(book);

            // Check auto genereted id
            Assert.AreNotEqual(0, book.Id);

            var selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));

            var books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(Helper.Comparer.IsSameBook(book, books.First()));

            book.Price = 3.19m;
            book.Title = "Book Test fields update";

            // Act
            // Update only the Title
            entityFactory.Update(book, nameof(Book.Title));

            // Assert
            selectBuilder = entityFactory.CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));

            books = selectBuilder.Execute();
            Assert.AreEqual(1, books.Count);

            // Updated only title, price should be not the same
            Assert.AreEqual(book.Title, books[0].Title);
            Assert.AreNotEqual(book.Price, books[0].Price);
        }
    }
}
