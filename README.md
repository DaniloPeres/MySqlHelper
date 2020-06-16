<p align="center">
  <img src="http://daniloperes.com/MySQL_Helper_Logo.svg" alt="MySQL Helper logo" width="120" height="120">
</p>

# MySqlHelper
<b>MySQL Helper</b> is a extension of MySQL to build queries or using entity models to interact with the database

## Table of Contents

- [Entity Model to MySQL](#entity-model-to-mysql)
  - [Select all books](#select-all-books)
- [Reporting Bugs and Issues](#reporting-bugs-and-issues)
- [Reporting Security Issues and Responsible Disclosure](#reporting-security-issues-and-responsible-disclosure)
- [Contributing](#contributing)
- [Platform, Build and Deployment Status](#platform-build-and-deployment-status)
- [License](#license)

## Entity Model to MySQL

Let's use the models Book and Publisher as example:

```csharp
[Table("books")]
public class Book
{
    [Key(AutoIncrement = true)]
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int PublisherId { get; set; }
    [ForeignKeyModel]
    public Publisher Publisher { get; set; }
}

[Table("publishers")]
public class Publisher
{
    [Key(AutoIncrement = true)]
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Select
#### Select all books:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory.CreateSelectBuilder<Book>();
IList<Book> books = selectBuilder.Execute();
```

#### Select a book by ID:

```csharp
var searchId = 1;
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), searchId));
IList<Book> books = selectBuilder.Execute();
```

#### Select books by complex filter (Price is lower than 10.00 and the Title contains 'Paris':

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithWhere(new WhereQueryLowerThan(GetColumnName<Book>(nameof(Book.Price)), 10d),
      (WhereQuerySyntaxEnum.And, new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%Paris%")));
IList<Book> books = selectBuilder.Execute();
```

#### Select all books with sub entity model Publisher (Left Join):

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithJoin(
          JoinEnum.LeftJoin,
          GetTableName<Book>(),
          GetTableName<Publisher>(),
          (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id)))));
IList<Book> books = selectBuilder.Execute();
```

#### Select all books only the column Book.Title and Publisher.Name:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithColumns<Book>(GetColumnName<Book>(nameof(Book.Title)))
   .WithColumns<Publisher>(GetColumnName<Publisher>(nameof(Publisher.Name)))
   .WithJoin(
          JoinEnum.LeftJoin,
          GetTableName<Book>(),
          GetTableName<Publisher>(),
          (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id)))));
IList<Book> books = selectBuilder.Execute();
```

#### Select all books in order descendent by ID:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithOrderBy((GetColumnName<Book>(nameof(Book.Id)), OrderBySorted.Desc));
IList<Book> books = selectBuilder.Execute();
```

#### Select books filter by Publisher Name:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithJoin(
       JoinEnum.LeftJoin,
       GetTableName<Book>(),
       GetTableName<Publisher>(),
       (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id))))
   .WithWhere<Publisher>(new WhereQueryEquals(GetColumnName<Publisher>(nameof(Publisher.Name)), "Publisher Name"));
IList<Book> books = selectBuilder.Execute();
```

### Insert
#### Insert a register by entity model:

```csharp
var publisher = new Publisher
{
    Id = 1,
    Name = "Publisher 1"
};
var book = new Book
{
    Id = 1,
    Title = "Book 1",
    Price = 9.99m,
    PublisherId = publisher.Id,
    Publisher = publisher
}

var entityFactory = new EntityFactory(<connectionString>);
entityFactory.Insert(publisher);
entityFactory.Insert(book);
```

#### Insert multiple registers by entity models:

```csharp
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

var entityFactory = new EntityFactory(<connectionString>);
entityFactory.InsertMultiples(publishers);
entityFactory.InsertMultiples(books);
```

### Update
#### Update a register by entity model:
```csharp
var entityFactory = new EntityFactory(<connectionString>);
var book = new Book
{
    Title = "Book Test new",
    Price = 1.99m
};

// Insert the register
entityFactory.Insert(book);

// Change the entity model
book.Price = 3.99m;
book.Title = "Book Test update";

// Update it in the database
entityFactory.Update(book);
```

### Update
```csharp
var entityFactory = new EntityFactory(<connectionString>);
var book = new Book
{
    Title = "Book Test new",
    Price = 1.99m
};

// Insert the register
entityFactory.Insert(book);

// Delete the register
entityFactory.Delete(book);
```

## Building Queries

<b>MySQL Helper</b> can generate queries with or without model

### Select

#### Select all books using model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder();
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books`"
```

#### Select all books without model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder();
string query = selectQueryBuilder.Build("books");

// query is going to be: "SELECT * FROM `books`"
```

#### Select all books for specific columns using model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithColumns(GetColumnName<Book>(nameof(Book.Title)), GetColumnName<Book>(nameof(Book.Price)));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT `Title`, `Price` FROM `books`"
```


#### Select all books for specific columns without model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithColumns("Title", "Price");
string query = selectQueryBuilder.Build("books");

// query is going to be: "SELECT `Title`, `Price` FROM `books`"
```

#### Select a book by ID using model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Id` = 1"
```

#### Select a book by ID without model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryEquals("Id", 1));
string query = selectQueryBuilder.Build("books");

// query is going to be: "SELECT * FROM `books` WHERE `Id` = 1"
```

#### Select books with 2 conditions using model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(
      new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1),
      (WhereQuerySyntaxEnum.Or, new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 2)));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Id` = 1 OR `Id` = 2"
```

#### Select books with 2 conditions without model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(
      new WhereQueryEquals("Id", 1),
      (WhereQuerySyntaxEnum.Or, new WhereQueryEquals("Id", 2)));
string query = selectQueryBuilder.Build("books");

// query is going to be: "SELECT * FROM `books` WHERE `Id` = 1 OR `Id` = 2"
```

#### Select books with complex 'where' using model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryBetween(GetColumnName<Book>(nameof(Book.Price)), 50, 100))
  .WithWhereAppend(
      WhereQuerySyntaxEnum.And,
      new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%C#%"),
      (WhereQuerySyntaxEnum.Or,
          new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%MySql%")));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE (`Price` BETWEEN 50 AND 100) AND (`Title` LIKE '%C#%' OR `Title` LIKE '%MySql%')"
```

#### Select books with complex 'WHERE' without model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryBetween("Price", 50, 100))
  .WithWhereAppend(
      WhereQuerySyntaxEnum.And,
      new WhereQueryLike("Title", "%C#%"),
      (WhereQuerySyntaxEnum.Or, new WhereQueryLike("Title", "%MySql%")));
string query = selectQueryBuilder.Build("books");

// query is going to be: "SELECT * FROM `books` WHERE (`Price` BETWEEN 50 AND 100) AND (`Title` LIKE '%C#%' OR `Title` LIKE '%MySql%')"
```

#### Select books with 'LIKE' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryBetween(GetColumnName<Book>(nameof(Book.Price)), 50, 100));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Price` BETWEEN 50 AND 100"
```

#### Select books with 'between' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryLike(GetColumnName<Book>(nameof(Book.Title)), "%C#%"));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Title` LIKE '%C#%'"
```

#### Select books with 'IN' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryIn(GetColumnName<Book>(nameof(Book.Id)), 1, 2, 3));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Id` IN (1,2,3)"
```

#### Select books with 'greater' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryGreaterThan(GetColumnName<Book>(nameof(Book.Price)), 100));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Price` > 100"
```

#### Select books with 'LEFT JOIN' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithColumns<Book>(GetColumnName<Book>(nameof(Book.Title)))
  .WithColumns<Publisher>(GetColumnName<Publisher>(nameof(Publisher.Name)))
  .WithJoin(
      JoinEnum.LeftJoin,
      GetTableName<Book>(),
      GetTableName<Publisher>(),
      (GetColumnName<Book>(nameof(Book.PublisherId)), GetColumnName<Publisher>(nameof(Publisher.Id))))
  .WithWhere<Book>(new WhereQueryGreaterThan(GetColumnName<Book>(nameof(Book.Price)), 100));
string query = selectQueryBuilder.Build<Book>();

/* query is going to be: 
  "SELECT `books`.`Title`, `publishers`.`Name`,
   FROM `books`
   LEFT JOIN `publishers` ON `books`.`PublisherId` = `publishers`.`Id`
   WHERE `books`.`Price` > 100"
*/
```

#### Select books with 'GROUP BY' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
    .WithColumns("Count(*)", GetColumnName<Book>(nameof(Book.Title)))
    .WithGroupBy(GetColumnName<Book>(nameof(Book.Price)));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT Count(*), `Title` FROM `books` GROUP BY `Price`"
```

#### Select books with 'ORDER BY' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
    .WithOrderBy((GetColumnName<Book>(nameof(Book.Price)), OrderBySorted.Desc), (GetColumnName<Book>(nameof(Book.Title)), OrderBySorted.Asc));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` ORDER BY `Price` DESC, `Title` ASC"
```

### Insert
```csharp
var fields = new Dictionary<string, object>
{
    { GetColumnName<Book>(nameof(Book.Title)), "Essential C#" },
    { GetColumnName<Book>(nameof(Book.Price)), 20.99d }
};
var insertQueryBuilder = new InsertQueryBuilder()
    .WithFields(fields);

string query = insertQueryBuilder.Build<Book>();

// query is going to be: "INSERT INTO `books` (`Title`, `Price`) VALUES ('Essential C#', 20.99)"
```

### Delete

#### Delete all books:
```csharp
var deleteQueryBuilder = new DeleteQueryBuilder();
string query = deleteQueryBuilder.Build<Book>();

// query is going to be: "DELETE FROM `books`"
```

#### Delete the book by ID:
```csharp
var deleteQueryBuilder = new DeleteQueryBuilder()
  .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));
string query = deleteQueryBuilder.Build<Book>();

// query is going to be: "DELETE FROM `books` WHERE `Id` = 1"
```

### Update
```csharp
var fields = new Dictionary<string, object>
{
    { GetColumnName<Book>(nameof(Book.Title)), "Essential C#" },
    { GetColumnName<Book>(nameof(Book.Price)), 20.99d }
};
var updateQueryBuilder = new UpdateQueryBuilder()
    .WithFields(fields)
    .WithWhere(new WhereQueryEquals(GetColumnName<Book>(nameof(Book.Id)), 1));

string query = updateQueryBuilder.Build<Book>();

// query is going to be: "UPDATE `books` SET `Title` = 'Essential C#', `Price` = 20.99 WHERE `Id` = 1"
```

## Attributes for model
