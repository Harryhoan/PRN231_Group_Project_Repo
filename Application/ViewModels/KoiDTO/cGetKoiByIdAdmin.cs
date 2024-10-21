using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.KoiDTO
{
    public class cGetKoiByIdAdmin
    {
        public int Id { get; set; }       
        public int Size { get; set; }
        public DateTime Dob { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Categoryid { get; set; }
    }
}
