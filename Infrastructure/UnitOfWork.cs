using Application;
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

        public UnitOfWork(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }
        public Task<int> SaveChangeAsync()
        {
            return _apiContext.SaveChangesAsync();
        }
    }
}
