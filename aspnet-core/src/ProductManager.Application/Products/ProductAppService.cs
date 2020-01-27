using System.Linq;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using ProductManager.Entities;
using ProductManager.Products.Dto;

namespace ProductManager.Products
{
    public class ProductAppService :
        AsyncCrudAppService<Product, ProductDto, int, PagedProductResultRequestDto, CrateProductDto, ProductDto>,
        IProductAppService
    {
        public ProductAppService(IRepository<Product, int> repository) : base(repository)
        {
        }

        protected override IQueryable<Product> CreateFilteredQuery(PagedProductResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.Code.Contains(input.Keyword) ||
                         x.Name.Contains(input.Keyword));
        }
    }
}