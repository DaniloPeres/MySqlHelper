using MySqlHelper.Attributes;

namespace MySqlHelper.UnitTests.Models
{
    [Table("publishers")]
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
