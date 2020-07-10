using MySqlHelper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.IntegrationTests.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key(AutoIncrement = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKeyModel]
        public List<Order> Orders { get; set; }
    }
}
