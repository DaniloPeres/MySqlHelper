using System.Collections.Generic;
using System.Linq;
using MySqlHelper.Entity;
using MySqlHelper.IntegrationTests.Configuration;
using MySqlHelper.IntegrationTests.Models;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using NUnit.Framework;
using static MySqlHelper.Attributes.ColumnAttribute;
using static MySqlHelper.Attributes.TableAttribute;

namespace MySqlHelper.IntegrationTests.Entity
{
    [TestFixture]
    public class SelectEntityRunner
    {
        private EntityFactory entityFactory;
        private List<Book> books;
        private List<Publisher> publishers;

        [SetUp]
        public void SetUp()
        {
            Helper.DataBase.CleanTable<Book>();
            Helper.DataBase.CleanTable<Publisher>();
            var config = new ConfigurationSettings();
            entityFactory = new EntityFactory(config.ConnectionString);
            SetDefaultRegisters();
            InsertDataForTests();
        }

        [Test]
        public void SelectAllRegisters()
        {
            var selectBuilder = entityFactory.CreateSelectBuilder<Book>();
            var books = selectBuilder.Execute().ToList();

            Assert.AreEqual(2, books.Count);
            books.ForEach(book =>
            {
                Assert.True(this.books.Exists(x => IsSameBook(x, book)));
            });
        }

        [Test]
        public void SelectRegisterById()
        {
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));
            var books = selectBuilder.Execute();

            Assert.AreEqual(1, books.Count);
            Assert.AreEqual(1, books[0].Id);
        }

        [Test]
        public void SelectRegisterByComplexFilter()
        {
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Book>()
                .WithWhere(new WhereQueryLowerThan(GetColumnName<Book>(nameof(Book.Price)), 10d),
                    (WhereQuerySyntaxEnum.And, new WhereQueryEquals(GetColumnName<Book>(nameof(Book.PublisherId)), 1)));
            var books = selectBuilder.Execute();

            Assert.AreEqual(1, books.Count);
        }

        [Test]
        public void SelectRegisterWithLeftJoin()
        {
            var book = this.books.First();
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Book>()
                .WithJoin(
                    JoinEnum.LeftJoin,
                    GetTableName<Book>(),
                    GetTableName<Publisher>(),
                    (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id))))
                .WithWhere<Book>(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), book.Id));

            var books = selectBuilder.Execute();

            Assert.AreEqual(1, books.Count);
            Assert.IsTrue(IsSameBook(book, books.First()));
            Assert.AreEqual(book.PublisherId, book.Publisher.Id);
            Assert.IsTrue(IsSamePublisher(book.Publisher, books.First().Publisher));
        }


        [Test]
        public void SelectRegisterWithLeftJoinAndSpecificColumns()
        {
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Book>()
                .WithColumns<Book>(GetColumnName<Book>(nameof(Book.Title)))
                .WithColumns<Publisher>(GetColumnName<Publisher>(nameof(Publisher.Name)))
                .WithJoin(
                    JoinEnum.LeftJoin,
                    GetTableName<Book>(),
                    GetTableName<Publisher>(),
                    (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id))))
                .WithWhere<Book>(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));

            var books = selectBuilder.Execute();

            Assert.AreEqual(1, books.Count);
            var book = books.First();
            Assert.NotNull(book.Publisher);
            // Test if the others columns are the default values
            Assert.AreEqual(default(int), book.PublisherId);
            Assert.AreEqual(default(decimal), book.Price);
            Assert.AreEqual(default(int), book.Publisher.Id);
        }

        [Test]
        public void SelectRegisterWithOrderByDesc()
        {
            // TODO finish it, make insert, test and delete
            var selectBuilder = entityFactory
                .CreateSelectBuilder<Book>()
                .WithOrderBy((GetColumnName<Book>(nameof(Book.Id)), OrderBySorted.Desc));

            var books = selectBuilder.Execute();

            Assert.AreEqual(2, books.Count);
            Assert.Greater(books[0].Id, books[1].Id);
        }

        [Test]
        public void SelectCountRegisterPerAuthor()
        {
            // TODO
            Assert.Fail();
        }

        private void SetDefaultRegisters()
        {
            publishers = new List<Publisher>
            {
                new Publisher
                {
                    Id = 1,
                    Name = "Publisher 1"
                },
                new Publisher
                {
                    Id = 2,
                    Name = "Publisher 2"
                }
            };
            books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Book 1",
                    Price = 9.99m,
                    PublisherId = publishers[0].Id,
                    Publisher = publishers[0]
                },
                new Book
                {
                    Id = 2,
                    Title = "Book 2",
                    Price = 19.99m,
                    PublisherId = publishers[1].Id,
                    Publisher = publishers[1]
                }
            };
        }

        private void InsertDataForTests()
        {
            entityFactory.Insert(publishers);
            entityFactory.Insert(books);
        }

        private static bool IsSameBook(Book book1, Book book2)
        {
            return
                book1.Id.Equals(book2.Id)
                && book1.Title.Equals(book2.Title)
                && book1.Price.Equals(book2.Price)
                && book1.PublisherId.Equals(book2.PublisherId);
        }

        private static bool IsSamePublisher(Publisher publisher1, Publisher publisher2)
        {
            return
                publisher1.Id.Equals(publisher2.Id)
                && publisher1.Name.Equals(publisher2.Name);
        }
    }
}
