using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CheckDuplicateAddress(string province, string street, string ward, string district)
        {
            var addressList = await _context.Addresses.ToListAsync();
            if (addressList == null || addressList.Count == 0)
            {
                return false;
            }
            return addressList.Any(a => AreStringsSimilar(a.Ward, ward) && AreStringsSimilar(a.Province, province) && AreStringsSimilar(a.District, district) && AreStringsSimilar(a.Street, street));
        }

        public static bool AreStringsSimilar(string input1, string input2)
        {
            if (string.IsNullOrWhiteSpace(input1) || string.IsNullOrWhiteSpace(input2))
            {
                return false;
            }
            input1 = input1.Trim().ToLower();
            input2 = input2.Trim().ToLower();
            return input1 == input2 || input1.Contains(input2) || input2.Contains(input1);
        }
    }


}
