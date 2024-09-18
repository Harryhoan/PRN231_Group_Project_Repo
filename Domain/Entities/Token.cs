using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TokenValue { get; set; }
        public string Type { get; set; }

        // Foreign key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
