using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
	public interface IImageRepo : IGenericRepo<Image>
	{
		Task<List<Image>> aGetImagesByKoiIdAsync(int koiId);
	}
}
