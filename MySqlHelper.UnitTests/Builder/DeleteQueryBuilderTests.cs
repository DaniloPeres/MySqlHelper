using NUnit.Framework;
using MySqlHelper.QueryBuilder;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using MySqlHelper.UnitTests.Models;
using static MySqlHelper.Attributes.TableAttribute;
using static MySqlHelper.Attributes.ColumnAttribute;

namespace MySqlHelper.UnitTests.Builder
{
    [TestFixture]
    public class DeleteQueryBuilderTests
    {
        [Test]
        public void GenerateSimpleQueryTest()
        {
            // Arrange
            const string queryExpected = "DELETE FROM `books`";
            var deleteQueryBuilder = new DeleteQueryBuilder();

            // Act
            var query = deleteQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQuerySimpleWhereTest()
        {
            // Arrange
            const string queryExpected = "DELETE FROM `books` WHERE `Id` = 1";
            var deleteQueryBuilder = new DeleteQueryBuilder()
                .WithWhere(new WhereQueryEquals(GetColumnNameWithQuotes<Book>(nameof(Book.Id)), 1));

            // Act
            var query = deleteQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWith2WhereConditionsTest()
        {
            // Arrange
            const string queryExpected = "DELETE FROM `books` WHERE `Id` = 1 OR `Id` = 2";
            var deleteQueryBuilder =  new DeleteQueryBuilder()
                .WithWhere(
                    new WhereQueryEquals(GetColumnNameWithQuotes<Book>(nameof(Book.Id)), 1),
                    (WhereQuerySyntaxEnum.Or, new WhereQueryEquals(GetColumnNameWithQuotes<Book>(nameof(Book.Id)), 2)));

            // Act
            var query = deleteQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithJoinTest()
        {
            // Arrange
            const string queryExpected = "DELETE FROM `books` JOIN `publishers` ON `books`.`PublisherId` = `publishers`.`Id` WHERE `publishers`.`Name` LIKE '%Scholastic%'";
            var deleteQueryBuilder = new DeleteQueryBuilder()
                .WithJoin(
                    JoinEnum.Join,
                    GetTableNameWithQuotes<Book>(),
                    GetTableNameWithQuotes<Publisher>(),
                    (GetColumnNameWithQuotes<Book>(nameof(Book.PublisherId)), GetColumnNameWithQuotes<Publisher>(nameof(Publisher.Id))))
                .WithWhere<Publisher>(new WhereQueryLike(GetColumnNameWithQuotes<Publisher>(nameof(Publisher.Name)), "%Scholastic%"));

            // Act
            var query = deleteQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }
    }
}
