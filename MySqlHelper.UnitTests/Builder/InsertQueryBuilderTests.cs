using System.Collections.Generic;
using MySqlHelper.QueryBuilder;
using MySqlHelper.UnitTests.Models;
using NUnit.Framework;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.UnitTests.Builder
{
    [TestFixture]
    public class InsertQueryBuilderTests
    {
        [Test]
        public void GenerateSimpleQueryTest()
        {
            // Arrange
            const string queryExpected = "INSERT INTO `books` (`Title`, `Price`) VALUES ('Essential C#', 20.99)";
            var fields = new Dictionary<string, object>
            {
                { GetColumnNameWithQuotes<Book>(nameof(Book.Title)), "Essential C#" },
                { GetColumnNameWithQuotes<Book>(nameof(Book.Price)), 20.99d }
            };
            var insertQueryBuilder = new InsertQueryBuilder()
                .WithFields(fields);

            // Act
            var query = insertQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }
    }
}
