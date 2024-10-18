using Application.IRepositories;
using Application.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

	}
}
