using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ProductManager.Authorization.Roles;
using ProductManager.Authorization.Users;
using ProductManager.MultiTenancy;

namespace ProductManager.EntityFrameworkCore
{
    public class ProductManagerDbContext : AbpZeroDbContext<Tenant, Role, User, ProductManagerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options)
            : base(options)
        {
        }
    }
}
