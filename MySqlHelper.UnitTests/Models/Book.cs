using MySqlHelper.Attributes;

namespace MySqlHelper.UnitTests.Models
{
    [Table("books")]
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int PublisherId { get; set; }
        [ForeignKeyModel]
        public Publisher Publisher { get; set; }
    }
}
