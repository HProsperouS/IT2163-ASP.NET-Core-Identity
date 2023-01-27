using FreshFarmMarket_211283E.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreshFarmMarket_211283E.DataContext
{
    public class AuthDbContext: IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
		public DbSet<Log> Logs { get; set; }

		public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString");optionsBuilder.UseSqlServer(connectionString);
        }
       

    }
}
