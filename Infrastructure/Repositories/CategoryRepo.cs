﻿using Application.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly ApiContext _dbContext;
        public CategoryRepo(ApiContext context) : base(context)
        {
            _dbContext = context;
        }
    }
}