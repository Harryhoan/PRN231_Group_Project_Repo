using Application.ViewModels.KoiDTO;
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
        Task<List<Koi>> dGetFilteredKois(dFilterKoiDTO filter);
        Task<Koi?> dGetKoiWithCategory(int id);

    }
}
