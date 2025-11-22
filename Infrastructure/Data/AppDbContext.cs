using Microsoft.EntityFrameworkCore;
using Domain.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Data
{
    public class AppDbContext:IdentityDbContext<AppUser,IdentityRole,string>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Product>Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    }
}
