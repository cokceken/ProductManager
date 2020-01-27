using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductManager.Configuration;
using ProductManager.Web;

namespace ProductManager.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ProductManagerDbContextFactory : IDesignTimeDbContextFactory<ProductManagerDbContext>
    {
        public ProductManagerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ProductManagerDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            ProductManagerDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ProductManagerConsts.ConnectionStringName));

            return new ProductManagerDbContext(builder.Options);
        }
    }
}
