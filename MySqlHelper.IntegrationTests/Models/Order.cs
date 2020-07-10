using MySqlHelper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.IntegrationTests.Models
{
    [Table("order")]
    public class Order
    {
        [Key(AutoIncrement = true)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
    }
}
