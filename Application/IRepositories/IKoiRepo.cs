using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IKoiRepo : IGenericRepo<Koi>
    {
        Task cAddKoi(Koi koi); 
        Task<IEnumerable<Koi>> GetAllKOI();
    }
}
