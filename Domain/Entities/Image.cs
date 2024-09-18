using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        // Foreign key
        public int KoiProductId { get; set; }
        public Koi Koi { get; set; }
    }

}
