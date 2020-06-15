using MySqlHelper.Attributes;

namespace MySqlHelper.IntegrationTests.Models
{
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
}
