using Microsoft.EntityFrameworkCore;
using KoiFarmManagement.Models;
namespace KoiFarmManagement.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) 
        {

        }

    }
}
