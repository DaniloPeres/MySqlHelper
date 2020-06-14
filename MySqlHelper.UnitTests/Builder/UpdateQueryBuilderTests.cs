using System.Collections.Generic;
using MySqlHelper.QueryBuilder;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using MySqlHelper.UnitTests.Models;
using NUnit.Framework;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.UnitTests.Builder
{
    [TestFixture]
    public class UpdateQueryBuilderTests
    {
        [Test]
        public void GenerateSimpleQueryTest()
        {
            // Arrange
            const string queryExpected = "UPDATE `books` SET `Title` = 'Essential C#', `Price` = 20.99";
            var fields = new Dictionary<string, object>
            {
                { GetColumnName<Book>(nameof(Book.Title)), "Essential C#" },
                { GetColumnName<Book>(nameof(Book.Price)), 20.99d }
            };
            var updateQueryBuilder = new UpdateQueryBuilder()
                .WithFields(fields);

            // Act
            var query = updateQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateSimpleWhereQueryTest()
        {
            // Arrange
            const string queryExpected = "UPDATE `books` SET `Title` = 'Essential C#', `Price` = 20.99 WHERE `Id` = 1";
            var fields = new Dictionary<string, object>
            {
                { GetColumnName<Book>(nameof(Book.Title)), "Essential C#" },
                { GetColumnName<Book>(nameof(Book.Price)), 20.99d }
            };
            var updateQueryBuilder = new UpdateQueryBuilder()
                .WithFields(fields)
                .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));

            // Act
            var query = updateQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }
    }
}
