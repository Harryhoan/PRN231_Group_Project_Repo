using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }

        // Foreign key
        public int UserId { get; set; }
        public User User { get; set; }
        // Navigation property
        public Order Order { get; set; }
    }
}
