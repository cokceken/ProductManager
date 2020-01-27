using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ProductManager.Authorization.Roles;
using ProductManager.Authorization.Users;
using ProductManager.Entities;
using ProductManager.MultiTenancy;

namespace ProductManager.EntityFrameworkCore
{
    public class ProductManagerDbContext : AbpZeroDbContext<Tenant, Role, User, ProductManagerDbContext>
    {
        public DbSet<Product> Products { get; set; }
        
        public ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasAlternateKey(x => x.Code);
        }
    }
}
