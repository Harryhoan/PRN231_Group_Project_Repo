using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderStatus { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
        public int? AddressId { get; set; }
        // Foreign keys

        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation properties
        public Address Address { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
