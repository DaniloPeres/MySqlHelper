using MySqlHelper.Attributes;

namespace MySqlHelper.IntegrationTests.Models
{
    [Table("publishers")]
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
