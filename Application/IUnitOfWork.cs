using Application.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUnitOfWork
    {
        public IUserRepo UserRepository { get; }
        public IKoiRepo KoiRepository { get; }
        public ITokenRepo TokenRepo { get; }
        public ICategoryRepo CategoryRepo { get; }
        public IKoiRepo KoiRepo { get; }
        public Task<int> SaveChangeAsync();
    }
}
