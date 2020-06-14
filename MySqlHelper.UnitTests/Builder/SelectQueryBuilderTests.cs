using MySqlHelper.QueryBuilder;
using MySqlHelper.UnitTests.Models;
using NUnit.Framework;
using static MySqlHelper.Attributes.TableAttribute;
using static MySqlHelper.Attributes.ColumnAttribute;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;

namespace MySqlHelper.UnitTests.Builder
{
    [TestFixture]
    public class SelectQueryBuilderTests
    {
        [Test]
        public void GenerateSimpleQueryTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books`";
            var selectQueryBuilder = new SelectQueryBuilder();

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithSpecificColumnsTest()
        {
            // Arrange
            const string queryExpected = "SELECT `Title`, `Price` FROM `books`";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithColumns(GetColumnName<Book>(nameof(Book.Title)), GetColumnName<Book>(nameof(Book.Price)));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithSimpleWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Id` = 1";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWith2WhereConditionsTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Id` = 1 OR `Id` = 2";
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithWhere(
                    new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1),
                    (WhereQuerySyntaxEnum.Or, new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 2)));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithComplexWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE (`Price` BETWEEN 50 AND 100) AND (`Title` LIKE '%C#%' OR `Title` LIKE '%MySql%')";
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithWhere(new WhereQueryBetween(GetColumnName<Book>(nameof(Book.Price)), 50, 100))
                .WithWhereAppend(
                    WhereQuerySyntaxEnum.And,
                    new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%C#%"),
                    (WhereQuerySyntaxEnum.Or,
                        new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%MySql%")));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithEqualsWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Title` = 'Essential C#'";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Title)), "Essential C#"));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithNotEqualsWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE NOT `Price` = 0";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryNotEquals(GetColumnName<Book>(nameof(Book.Price)), 0d));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithLikeWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Title` LIKE '%C#%'";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%C#%"));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithNotLikeWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE NOT `Title` LIKE '%C#%'";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryNotLike(GetColumnName<Book>(nameof(Book.Title)), "%C#%"));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithBetweenWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Price` BETWEEN 50 AND 100";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryBetween(GetColumnName<Book>(nameof(Book.Price)), 50, 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithNotBetweenWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE NOT `Price` BETWEEN 50 AND 100";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryNotBetween(GetColumnName<Book>(nameof(Book.Price)), 50, 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithInWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Id` IN (1,2,3)";
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithWhere(new WhereQueryIn(GetColumnName<Book>(nameof(Book.Id)), 1, 2, 3));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithNotInWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE NOT `Id` IN (1,2,3)";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryNotIn(GetColumnName<Book>(nameof(Book.Id)), 1, 2, 3));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithGreaterThanWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Price` > 100";
            var selectQueryBuilder =  new SelectQueryBuilder()
                    .WithWhere(new WhereQueryGreaterThan(GetColumnName<Book>(nameof(Book.Price)), 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithGreaterOrEqualsThanWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Price` >= 100";
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithWhere(new WhereQueryGreaterThanOrEqual(GetColumnName<Book>(nameof(Book.Price)), 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithLowerThanWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Price` < 100";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryLowerThan(GetColumnName<Book>(nameof(Book.Price)), 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithLowerOrEqualsThanWhereTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` WHERE `Price` <= 100";
            var selectQueryBuilder = new SelectQueryBuilder()
                    .WithWhere(new WhereQueryLowerThanOrEqual(GetColumnName<Book>(nameof(Book.Price)), 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithJoinTest()
        {
            GenerateQueryWithJoinType(JoinEnum.Join, "JOIN");
        }

        [Test]
        public void GenerateQueryWithLeftJoinTest()
        {
            GenerateQueryWithJoinType(JoinEnum.LeftJoin, "LEFT JOIN");
        }

        [Test]
        public void GenerateQueryWithRightJoinTest()
        {
            GenerateQueryWithJoinType(JoinEnum.RightJoin, "RIGHT JOIN");
        }

        [Test]
        public void GenerateQueryWithFullJoinTest()
        {
            GenerateQueryWithJoinType(JoinEnum.FullJoin, "FULL JOIN");
        }

        [Test]
        public void GenerateQueryWithLeftJoinSpecificColumnsAndWhereTest()
        {
            // Arrange
            var queryExpected = string.Join(" ",
                "SELECT `books`.`Title`, `publishers`.`Name`",
                "FROM `books`",
                "LEFT JOIN `publishers` ON `books`.`PublisherId` = `publishers`.`Id`",
                "WHERE `books`.`Price` > 100");
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithColumns<Book>(GetColumnName<Book>(nameof(Book.Title)))
                .WithColumns<Publisher>(GetColumnName<Publisher>(nameof(Publisher.Name)))
                .WithJoin(
                    JoinEnum.LeftJoin,
                    GetTableName<Book>(),
                    GetTableName<Publisher>(),
                    (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id))))
                .WithWhere<Book>(new WhereQueryGreaterThan(GetColumnName<Book>(nameof(Book.Price)), 100));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithGroupByCategoryTest()
        {
            // Arrange
            const string queryExpected = "SELECT Count(*), `Title` FROM `books` GROUP BY `Price`";
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithColumns("Count(*)", GetColumnName<Book>(nameof(Book.Title)))
                .WithGroupBy(GetColumnName<Book>(nameof(Book.Price)));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        [Test]
        public void GenerateQueryWithOrderByTest()
        {
            // Arrange
            const string queryExpected = "SELECT * FROM `books` ORDER BY `Price` DESC, `Title` ASC";
            var selectQueryBuilder =  new SelectQueryBuilder()
                .WithOrderBy((GetColumnName<Book>(nameof(Book.Price)), OrderBySorted.Desc), (GetColumnName<Book>(nameof(Book.Title)), OrderBySorted.Asc));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }

        private void GenerateQueryWithJoinType(JoinEnum joinType, string joinString)
        {
            // Arrange
            var queryExpected = string.Join(" ",
                "SELECT",
                "`books`.`Id` `books_Id`, `books`.`Price` `books_Price`, `books`.`PublisherId` `books_PublisherId`, `books`.`Title` `books_Title`,",
                "`publishers`.`Id` `publishers_Id`, `publishers`.`Name` `publishers_Name`",
                "FROM `books`",
                joinString,
                "`publishers` ON `books`.`PublisherId` = `publishers`.`Id`");
            var selectQueryBuilder = new SelectQueryBuilder()
                .WithJoin(
                    joinType,
                    GetTableName<Book>(),
                    GetTableName<Publisher>(),
                    (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id))));

            // Act
            var query = selectQueryBuilder.Build<Book>();

            // Assert
            Assert.AreEqual(queryExpected, query);
        }
    }
}
