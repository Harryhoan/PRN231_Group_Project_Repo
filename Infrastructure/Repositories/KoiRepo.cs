using Application.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class KoiRepo : GenericRepo<Koi>, IKoiRepo
    {
        private readonly ApiContext _dbContext;
        public KoiRepo(ApiContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task cAddKoi(Koi koi)
        {
            try
            {
                if (koi == null)
                {
                    throw new ArgumentNullException(nameof(koi));
                }

                await _dbContext.Kois.AddAsync(koi);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the koi.", ex);
            }
        }
    }
}
