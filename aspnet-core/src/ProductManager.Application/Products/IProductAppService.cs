using Abp.Application.Services;
using ProductManager.Products.Dto;

namespace ProductManager.Products
{
    public interface IProductAppService : IAsyncCrudAppService<ProductDto, int, PagedProductResultRequestDto,
        CrateProductDto, ProductDto>
    {
    }
}