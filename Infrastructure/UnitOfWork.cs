using Application;
using Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext _apiContext;
        private readonly IUserRepo _userRepository;
        private readonly ITokenRepo _tokenRepo;
		private readonly IOrderRepo _orderRepo;
		public UnitOfWork(ApiContext apiContext, IUserRepo userRepository, ITokenRepo tokenRepo, IOrderRepo orderRepo)
		{
			//if (apiContext == null) throw new ArgumentNullException(nameof(apiContext));
			//if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
			//if (tokenRepo == null) throw new ArgumentNullException(nameof(tokenRepo));
			//_apiContext = apiContext;
			//_userRepository = userRepository;
			//_tokenRepo = tokenRepo;
			_apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			_tokenRepo = tokenRepo ?? throw new ArgumentNullException(nameof(tokenRepo));
			_orderRepo = orderRepo;
		}

		public IUserRepo UserRepository => _userRepository;

        public ITokenRepo TokenRepo => _tokenRepo;

        public IOrderRepo OrderRepository => _orderRepo;

        public async Task<int> SaveChangeAsync()
        {
            //return _apiContext.SaveChangesAsync();
            try
            {
                return await _apiContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception details here
                throw new ApplicationException("An error occurred while saving changes.", ex);
            }
        }
    }
}
