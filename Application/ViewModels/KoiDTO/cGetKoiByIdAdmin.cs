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
        public string Descriptionkoi { get; set; }
        public decimal Price { get; set; }
        public string Namekoi { get; set; }
        public int Quantity { get; set; }
        public int Categoryid { get; set; }
    }
}
