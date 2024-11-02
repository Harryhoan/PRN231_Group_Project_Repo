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
	public class ImageRepo : GenericRepo<Image>, IImageRepo
	{
		private readonly ApiContext _dbContext;
		public ImageRepo(ApiContext context) : base(context)
		{
			_dbContext = context;
		}

		public async Task<List<Image>> aGetImagesByKoiIdAsync(int koiId)
		{
			return await _dbContext.Images.Where(record => record.KoiId == koiId).ToListAsync();
		}
	}
}
