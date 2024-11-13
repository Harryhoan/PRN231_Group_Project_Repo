using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Foreign keys
        public int KoiId { get; set; }
        public Koi Koi { get; set; }
        public bool IsReviewed { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Review foreign key
        public Review Review { get; set; }
    }
}
