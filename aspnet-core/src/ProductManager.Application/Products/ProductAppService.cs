using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using ProductManager.Entities;
using ProductManager.Products.Dto;

namespace ProductManager.Products
{
    public class ProductAppService :
        AsyncCrudAppService<Product, ProductDto, int, PagedProductResultRequestDto, CreateProductDto, ProductDto>,
        IProductAppService
    {
        public ProductAppService(IRepository<Product, int> repository) : base(repository)
        {
            LocalizationSourceName = ProductManagerConsts.LocalizationSourceName;
        }

        protected override IQueryable<Product> CreateFilteredQuery(PagedProductResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.Code.Contains(input.Keyword) ||
                         x.Name.Contains(input.Keyword));
        }

        public override async Task<ProductDto> CreateAsync(CreateProductDto input)
        {
            if (CheckDuplicateProductCode(input.Code))
                throw new UserFriendlyException(L("ActionError"), L("ProductCodeDuplicate"));

            return await base.CreateAsync(input);
        }

        public override async Task<ProductDto> UpdateAsync(ProductDto input)
        {
            if (CheckDuplicateProductCode(input.Code, input.Id))
                throw new UserFriendlyException(L("ActionError"), L("ProductCodeDuplicate"));

            return await base.UpdateAsync(input);
        }

        private bool CheckDuplicateProductCode(string productCode, int id = -1)
        {
            return Repository.GetAll().Any(x => x.Code == productCode && x.Id != id);
        }
    }
}