using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        // Foreign keys
        public int OrderDetailId { get; set; }
        public OrderDetail OrderDetail { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
