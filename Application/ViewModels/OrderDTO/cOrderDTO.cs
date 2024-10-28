using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderDTO
{
    public class cOrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool Status { get; set; }
        public List<cOrderDetailsResDTO> OrderDetails { get; set; }
    }
    public class cOrderDetailsResDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
