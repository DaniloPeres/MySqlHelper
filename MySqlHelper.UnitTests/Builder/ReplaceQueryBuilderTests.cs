using System;
using System.Collections.Generic;
using System.Text;
using MySqlHelper.QueryBuilder;
using MySqlHelper.UnitTests.Models;
using NUnit.Framework;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.UnitTests.Builder
{
    [TestFixture]
    public class ReplaceQueryBuilderTests
    {
        [Test]
        public void GenerateSimpleQueryTest()
        {
            // Arrange
            const string queryExpected = "REPLACE INTO `books` (`Id`, `Title`, `Price`) VALUES (1, 'Essential C#', 20.99)";
            var fields = new Dictionary<string, object>
            {
                { GetColumnNameWithQuotes<Book>(nameof(Book.Id)), 1 },
                { GetColumnNameWithQuotes<Book>(nameof(Book.Title)), "Essential C#" },
                { GetColumnNameWithQuotes<Book>(nameof(Book.Price)), 20.99d }
            };
            var insertQueryBuilder = new ReplaceQueryBuilder()
                .WithFields(fields);

            // Act
            var query = insertQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }
    }
}
