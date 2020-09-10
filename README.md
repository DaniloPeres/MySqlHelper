<p align="center">
  <img src="http://daniloperes.com/MySQL_Helper_Logo_256.png" alt="MySQL Helper logo" width="120" height="120">
</p>

# MySqlHelper
<b>MySQL Helper</b> is an extension of MySQL to build queries or using entity models to interact with the database

## Nuget package
There is a nuget package avaliable here https://www.nuget.org/packages/DaniloPeres.MySqlHelper/.

## Table of Contents

- [Entity Model to MySQL](#entity-model-to-mysql)
  - [Select all books](#select-all-books)
  - [Select a book by ID](#select-a-book-by-id)
  - [Select books by complex filter (Price is lower than 10.00 and the Title contains 'Paris')](#select-books-by-complex-filter-price-is-lower-than-1000-and-the-title-contains-paris)
  - [Select all books with sub entity model Publisher (Left Join)](#select-all-books-with-sub-entity-model-publisher-left-join)
  - [Select all books only the column Book.Title and Publisher.Name](#select-all-books-only-the-columns-booktitle-and-publishername)
  - [Select all books in order descendent by ID](#select-all-books-in-order-descendent-by-id)
  - [Select books filter by Publisher Name](#select-books-filter-by-publisher-name)
  - [Insert a register by entity model](#insert-a-register-by-entity-model)
  - [Insert multiple registers by entity models](#insert-multiple-registers-by-entity-models)
  - [Update a register by entity model](#update-a-register-by-entity-model)
  - [Update register only specific fields by entity model](#update-register-only-specific-fields-by-entity-model)
  - [Replace a register by entity model](#replace-a-register-by-entity-model)
  - [Delete a register by entity model](#delete-a-register-by-entity-model)
  - [Select Item with sub-items](#select-item-with-sub-items)
- [Building Queries](#building-queries)
  - [Select all books using model](#select-all-books-using-model)
  - [Select all books without model](#select-all-books-without-model)
  - [Select all books for specific columns using model](#select-all-books-for-specific-columns-using-model)
  - [Select all books for specific columns without model](#select-all-books-for-specific-columns-without-model)
  - [Select a book by ID using model](#select-a-book-by-id-using-model)
  - [Select a book by ID without model](#select-a-book-by-id-without-model)
  - [Select books with 2 conditions using model](#select-books-with-2-conditions-using-model)
  - [Select books with 2 conditions without model](#select-books-with-2-conditions-without-model)
  - [Select books with complex 'WHERE' using model](#select-books-with-complex-where-using-model)
  - [Select books with complex 'WHERE' without model](#select-books-with-complex-where-without-model)
  - [Select books with 'LIKE' condition](#select-books-with-like-condition)
  - [Select books with 'between' condition](#select-books-with-between-condition)
  - [Select books with 'IN' condition](#select-books-with-in-condition)
  - [Select books with column 'IS NULL' condition](#select-books-with-column-is-null-condition)
  - [Select books with column 'IS NOT NULL' condition](#select-books-with-column-is-not-null-condition)
  - [Select books with 'greater' condition](#select-books-with-greater-condition)
  - [Select books with 'LEFT JOIN' condition](#select-books-with-left-join-condition)
  - [Select books with 'GROUP BY' condition](#select-books-with-group-by-condition)
  - [Select books with 'ORDER BY' condition](#select-books-with-order-by-condition)
  - [Insert query builder](#insert-query-builder)
  - [Replace query builder](#replace-query-builder)
  - [Delete all books](#delete-all-books) 
  - [Delete the book by ID](#delete-the-book-by-id)
  - [Update query builder](#update-query-builder)
- [Attributes for the model](#attributes-for-the-model)
  - [Table Attribute](#table-attribute)
  - [Column Attribute](#column-attribute)
  - [Key Attribute](#key-attribute)
  - [Foreign Key Model Attribute](#foreign-key-model-attribute)
  - [Foreign Key Id Attribute](#foreign-key-id-attribute)
  - [Ignore Attribute](#ignore-attribute)

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
   .WithWhere<Book>(new WhereQueryEquals(GetColumnNameWithQuotes<Book>(nameof(Book.Id)), book.Id));
IList<Book> books = selectBuilder.Execute();
```

#### Select books by complex filter (Price is lower than 10.00 and the Title contains 'Paris'):

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithWhere(new WhereQueryLowerThan(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 10d),
      (WhereQuerySyntaxEnum.And, new WhereQueryLike(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), "%Paris%")));
IList<Book> books = selectBuilder.Execute();
```

#### Select all books with sub entity model Publisher (Left Join):

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithJoin(
          JoinEnum.LeftJoin,
          TableAttribute.GetTableName<Book>(),
          TableAttribute.GetTableName<Publisher>(),
          (ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.PublisherId)), ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Id)))));
IList<Book> books = selectBuilder.Execute();
```

#### Select all books only the columns Book.Title and Publisher.Name:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithColumns<Book>(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)))
   .WithColumns<Publisher>(ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Name)))
   .WithJoin(
          JoinEnum.LeftJoin,
          TableAttribute.GetTableName<Book>(),
          TableAttribute.GetTableName<Publisher>(),
          (ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.PublisherId)), ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Id)))));
IList<Book> books = selectBuilder.Execute();
```

#### Select all books in order descendent by ID:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithOrderBy((ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), OrderBySorted.Desc));
IList<Book> books = selectBuilder.Execute();
```

#### Select books filter by Publisher Name:

```csharp
var entityFactory = new EntityFactory(<connectionString>);
var selectBuilder = entityFactory
   .CreateSelectBuilder<Book>()
   .WithJoin(
       JoinEnum.LeftJoin,
       TableAttribute.GetTableName<Book>(),
       TableAttribute.GetTableName<Publisher>(),
       (ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.PublisherId)), ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Id))))
   .WithWhere<Publisher>(new WhereQueryEquals(ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Name)), "Publisher Name"));
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

#### Update register only specific fields by entity model:
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
book.Title = "Book Test update";

// Update only the title in the database
entityFactory.Update(book, nameof(Book.Title));
```

### Replace
#### Replace a register by entity model:

```csharp
var book = new Book
{
    Id = 1,
    Title = "Book 1",
    Price = 9.99m,
    PublisherId = publisher.Id,
    Publisher = publisher
}

var entityFactory = new EntityFactory(<connectionString>);
entityFactory.Replace(book);
```

### Delete a register by entity model:
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

### Select Item with sub-items:

Let's use the models Customer and Order as example:

```csharp
[Table("customer")]
public class Customer
{
    [Key(AutoIncrement = true)]
    public int Id { get; set; }
    public string Name { get; set; }
    [ForeignKeyModel]
    public List<Order> Orders { get; set; }

    public Customer()
    {
        Orders = new List<Order>();
    }
}

[Table("order")]
public class Order
{
    [Key(AutoIncrement = true)]
    public int Id { get; set; }
    [ForeignKeyId(typeof(Customer))]
    public int CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
}
```

We can select the customer and the list of orders using sub-items:

```csharp
var selectBuilder = entityFactory
    .CreateSelectBuilder<Customer>()
    .WithSubItems(typeof(Order));

List<Customer> customers = selectBuilder.Execute();
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
  .WithColumns(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)));
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
  .WithWhere(new WhereQueryEquals(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), 1));
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
      new WhereQueryEquals(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), 1),
      (WhereQuerySyntaxEnum.Or, new WhereQueryEquals(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), 2)));
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

#### Select books with complex 'WHERE' using model:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryBetween(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 50, 100))
  .WithWhereAppend(
      WhereQuerySyntaxEnum.And,
      new WhereQueryLike(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), "%C#%"),
      (WhereQuerySyntaxEnum.Or,
          new WhereQueryLike(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), "%MySql%")));
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
  .WithWhere(new WhereQueryBetween(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 50, 100));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Price` BETWEEN 50 AND 100"
```

#### Select books with 'BETWEEN' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryLike(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), "%C#%"));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Title` LIKE '%C#%'"
```

#### Select books with 'IN' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryIn(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), 1, 2, 3));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Id` IN (1,2,3)"
```

#### Select books with column 'IS NULL' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryIsNull(GetColumnNameWithQuotes<Book>(nameof(Book.Price))));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Price` IS NULL"
```

#### Select books with column 'IS NOT NULL' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryIsNotNull(GetColumnNameWithQuotes<Book>(nameof(Book.Price))));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE NOT `Price` IS NULL"
```

#### Select books with 'greater' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithWhere(new WhereQueryGreaterThan(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 100));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` WHERE `Price` > 100"
```

#### Select books with 'LEFT JOIN' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
  .WithColumns<Book>(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)))
  .WithColumns<Publisher>(ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Name)))
  .WithJoin(
      JoinEnum.LeftJoin,
      TableAttribute.GetTableName<Book>(),
      TableAttribute.GetTableName<Publisher>(),
      (ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.PublisherId)), ColumnAttribute.GetColumnName(typeof(Publisher), nameof(Publisher.Id))))
  .WithWhere<Book>(new WhereQueryGreaterThan(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 100));
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
    .WithColumns("Count(*)", ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)))
    .WithGroupBy(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT Count(*), `Title` FROM `books` GROUP BY `Price`"
```

#### Select books with 'ORDER BY' condition:
```csharp
var selectQueryBuilder = new SelectQueryBuilder()
    .WithOrderBy((ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), OrderBySorted.Desc), (ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), OrderBySorted.Asc));
string query = selectQueryBuilder.Build<Book>();

// query is going to be: "SELECT * FROM `books` ORDER BY `Price` DESC, `Title` ASC"
```

### Insert query builder
```csharp
var fields = new Dictionary<string, object>
{
    { ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), "Essential C#" },
    { ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 20.99d }
};
var insertQueryBuilder = new InsertQueryBuilder()
    .WithFields(fields);

string query = insertQueryBuilder.Build<Book>();

// query is going to be: "INSERT INTO `books` (`Title`, `Price`) VALUES ('Essential C#', 20.99)"
```

### Replace query builder
```csharp
var fields = new Dictionary<string, object>
{
    { ColumnAttribute.GetColumnNameWithQuotes<Book>(nameof(Book.Id)), 1 },
    { ColumnAttribute.GetColumnNameWithQuotes<Book>(nameof(Book.Title)), "Essential C#" },
    { ColumnAttribute.GetColumnNameWithQuotes<Book>(nameof(Book.Price)), 20.99d }
};
var insertQueryBuilder = new ReplaceQueryBuilder()
    .WithFields(fields);

string query = insertQueryBuilder.Build<Book>();

// query is going to be: "REPLACE INTO `books` (`Id`, `Title`, `Price`) VALUES (1, 'Essential C#', 20.99)"
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
  .WithWhere(new WhereQueryEquals(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), 1));
string query = deleteQueryBuilder.Build<Book>();

// query is going to be: "DELETE FROM `books` WHERE `Id` = 1"
```

### Update query builder
```csharp
var fields = new Dictionary<string, object>
{
    { ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Title)), "Essential C#" },
    { ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Price)), 20.99d }
};
var updateQueryBuilder = new UpdateQueryBuilder()
    .WithFields(fields)
    .WithWhere(new WhereQueryEquals(ColumnAttribute.GetColumnName(typeof(Book), nameof(Book.Id)), 1));

string query = updateQueryBuilder.Build<Book>();

// query is going to be: "UPDATE `books` SET `Title` = 'Essential C#', `Price` = 20.99 WHERE `Id` = 1"
```

## Attributes for the model

### Table Attribute
<b>Attribute Targets:</b> Class<br/>
<b>Parameters:</b>
  - Name

<b>Description:</b> Set the table name from this model
<b>Examples:</b>
```csharp
[Table("books")]
public class Book
{...}
```

### Column Attribute
<b>Attribute Targets:</b> Property<br/>
<b>Parameters:</b>
  - Name
  - Type

<b>Description:</b> Set column properties<br/>
<b>Examples:</b>
```csharp
[Column("ID")]
public int Id { get; set; }

[Column("Date", ColumnTypeEnum.Date)]
public DateTime Date { get; set; }

[Column(ColumnTypeEnum.Time)]
public DateTime Time { get; set; }
```

### Key Attribute
<b>Attribute Targets:</b> Property<br/>
<b>Parameters:</b>
  - AutoIncrement

<b>Description:</b> Set the column as primary key<br/>
<b>Examples:</b>
```csharp
[Key(AutoIncrement = true)]
public int Id { get; set; }

[Key]
public int Id { get; set; }
```

### Foreign Key Model Attribute
<b>Attribute Targets:</b> Property<br/>
<b>Description:</b> Set a class as model from another table<br/>
<b>Examples:</b>
```csharp
[ForeignKeyModel]
public Publisher Publisher { get; set; }
```

### Foreign Key Id Attribute
<b>Attribute Targets:</b> Property<br/>
<b>Parameters:</b>
  - ForeignType
  
<b>Description:</b> Set a property as ID from another model<br/>
<b>Examples:</b>
```csharp
[ForeignKeyId(typeof(Customer))]
public int CustomerId { get; set; }
```

### Ignore Attribute
<b>Attribute Targets:</b> Property<br/>
<b>Description:</b> Set this property to not be processed in MySQL Helper<br/>
<b>Examples:</b>
```csharp
[Ignore]
public Publisher Publisher { get; set; }
```


## License

MIT
