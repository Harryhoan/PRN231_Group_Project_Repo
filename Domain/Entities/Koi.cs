using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Koi
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public DateTime Dob { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // Navigation properties
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Image> Images { get; set; }
    }

}
