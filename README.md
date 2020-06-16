<p align="center">
  <img src="http://daniloperes.com/MySQL_Helper_Logo.svg" alt="MySQL Helper logo" width="120" height="120">
</p>

# MySqlHelper
<b>MySQL Helper</b> is a extension of MySQL to build queries or using entity models to interact with the database

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

## Building Queries


## Attributes for model
