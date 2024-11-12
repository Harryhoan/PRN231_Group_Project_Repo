using Application.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AddressRepo : GenericRepo<Address>, IAddressRepo
    {
        private readonly ApiContext _dbContext;
        public AddressRepo(ApiContext context) : base(context)
        {
            _dbContext = context;
        }
    }


}
