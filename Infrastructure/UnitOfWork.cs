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
        private readonly IKoiRepo _koiRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly IOrderDetailRepo _orderDetailRepo;
        private readonly IImageRepo _imageRepo;
        private readonly IAddressRepo _addressRepo;
        public UnitOfWork(ApiContext apiContext, IUserRepo userRepository, IAddressRepo addressRepo,
            ITokenRepo tokenRepo, ICategoryRepo categoryRepo, IKoiRepo koiRepo, IOrderRepo orderRepo, IReviewRepo reviewRepo, IOrderDetailRepo orderDetailRepo, IImageRepo imageRepo)
        {
            //if (apiContext == null) throw new ArgumentNullException(nameof(apiContext));
            //if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            //if (tokenRepo == null) throw new ArgumentNullException(nameof(tokenRepo));
            //_apiContext = apiContext;
            //_userRepository = userRepository;
            //_tokenRepo = tokenRepo;
            _addressRepo = addressRepo ?? throw new ArgumentNullException(nameof(addressRepo));
            _apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenRepo = tokenRepo ?? throw new ArgumentNullException(nameof(tokenRepo));
           _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
			_koiRepo = koiRepo ?? throw new ArgumentNullException(nameof(koiRepo));
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _reviewRepo = reviewRepo ?? throw new ArgumentNullException(nameof(reviewRepo));
			_orderDetailRepo = orderDetailRepo ?? throw new ArgumentNullException(nameof(orderDetailRepo));
            _imageRepo = imageRepo ?? throw new ArgumentNullException(nameof(imageRepo));
		}
		public IUserRepo UserRepository => _userRepository;

		public ITokenRepo TokenRepo => _tokenRepo;
        public IAddressRepo AddressRepo => _addressRepo;

        public ICategoryRepo CategoryRepo => _categoryRepo;

		public IKoiRepo KoiRepo => _koiRepo;

        public IOrderRepo OrderRepository => _orderRepo;

        public IReviewRepo ReviewRepository => _reviewRepo;

        public IOrderDetailRepo OrderDetailRepository => _orderDetailRepo;

        public IImageRepo ImageRepository => _imageRepo;

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
