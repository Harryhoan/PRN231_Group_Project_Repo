﻿using Application.IRepositories;
using Application.ViewModels.KoiDTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Koi>> GetAllKOI()
        {
            return await _dbContext.Kois
                .Include(p => p.Images)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Koi>> dGetFilteredKois(dFilterKoiDTO filter)
        {
            try
            {
                var query = _dbContext.Kois.AsNoTracking().Include(k => k.Category).Include(k => k.Images).AsQueryable();
                if (query == null)
                {
                    throw new ArgumentNullException("Finding any Koi produces an invalid result.");
                }
                if (query.Any())
                {
                    if (filter.Id.HasValue)
                    {
                        query = query.Where(k => k.Id == filter.Id.Value);
                    }

					if (!string.IsNullOrEmpty(filter.Name))
					{
						query = query.Where(k => k.Name.Contains(filter.Name));
					}

					if (!string.IsNullOrEmpty(filter.Description))
                    {
                        query = query.Where(k => k.Description.Contains(filter.Description));
                    }

                    if (filter.MinPrice.HasValue)
                    {
                        query = query.Where(k => k.Price >= filter.MinPrice.Value);
                    }

                    if (filter.MaxPrice.HasValue)
                    {
                        query = query.Where(k => k.Price <= filter.MaxPrice.Value);
                    }

                    if (filter.MinSize.HasValue)
                    {
                        query = query.Where(k => k.Size >= filter.MinSize.Value);
                    }

                    if (filter.MaxSize.HasValue)
                    {
                        query = query.Where(k => k.Size <= filter.MaxSize.Value);
                    }

                    if (filter.DobStart.HasValue)
                    {
                        query = query.Where(k => k.Dob >= filter.DobStart.Value);
                    }

                    if (filter.DobEnd.HasValue)
                    {
                        query = query.Where(k => k.Dob <= filter.DobEnd.Value);
                    }

                    //if (filter.CategoryId.HasValue)
                    //{
                    //    query = query.Where(k => k.CategoryId == filter.CategoryId.Value);
                    //}

					if (!string.IsNullOrWhiteSpace(filter.CategoryName))
					{
						query = query.Where(k => k.Category.Name.Contains(filter.CategoryName));
					}
				}
                return await query.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the kois.", ex);
            }
        }

        public async Task<Koi?> dGetKoiWithCategoryAndImages(int id)
        {
            return await _dbContext.Kois.Include(record => record.Category).Include(record => record.Images).SingleOrDefaultAsync(record => record.Id == id);
        }

        public async Task<Koi> cGetProductById(int id)
        {
            var product = await _dbContext.Kois
                 .Include(p => p.Images)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            return product;
        }

        public async Task cUpdateProduct(Koi product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                _dbContext.Entry(product).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        public async Task<Koi> cGetProductNotImage(int id)
        {
            var product = await _dbContext.Kois               
               .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            return product;
        }

        public async Task cDeleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.Kois.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found.");
                }

                _dbContext.Kois.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product.", ex);
            }
        }

        public async Task<List<Image>> GetImagesByKoiIdForAdmin(int koiId)
        {
            return await _context.Images
           .Where(image => image.KoiId == koiId)
           .ToListAsync();
        }

        public int GetKoiCount()
        {
            return _dbContext.Kois.Count();
        }
    }
}