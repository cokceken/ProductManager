using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using ProductManager.Entities;
using ProductManager.Products;
using ProductManager.Products.Dto;
using Shouldly;
using Xunit;

namespace ProductManager.Tests.Products
{
    public class ProductAppServiceTests : ProductManagerTestBase
    {
        private readonly IProductAppService _productAppService;

        public ProductAppServiceTests()
        {
            _productAppService = LocalIocManager.Resolve<IProductAppService>();
        }

        [Fact]
        public async Task GetAll_FiltersWithCode_WhenKeywordProvided()
        {
            CreateData();
            var output = await _productAppService.GetAllAsync(new PagedProductResultRequestDto()
                {Keyword = "Code1", MaxResultCount = 10, SkipCount = 0});

            output.Items.Count.ShouldBe(1);
            output.Items.First().Id.ShouldBe(1);
        }

        [Fact]
        public async Task GetAll_FiltersWithName_WhenKeywordProvided()
        {
            CreateData();
            var output = await _productAppService.GetAllAsync(new PagedProductResultRequestDto()
                {Keyword = "Name1", MaxResultCount = 10, SkipCount = 0});

            output.Items.Count.ShouldBe(1);
            output.Items.First().Id.ShouldBe(1);
        }

        [Fact]
        public async Task Create_ShouldThrowUserFriendlyException_WhenCodeIsDuplicated()
        {
            CreateData();
            var ex = await Record.ExceptionAsync(() => _productAppService.CreateAsync(new CreateProductDto()
            {
                Code = "Code1",
                Name = "Name",
                Price = 1
            }));

            var userFriendlyException = ex as UserFriendlyException;
            userFriendlyException.ShouldNotBeNull();
        }

        [Fact]
        public async Task Update_ShouldThrowUserFriendlyException_WhenCodeIsDuplicated()
        {
            CreateData();
            var ex = await Record.ExceptionAsync(() => _productAppService.UpdateAsync(new ProductDto()
            {
                Code = "Code1",
                Name = "Name",
                Price = 1,
                Id = 3
            }));

            var userFriendlyException = ex as UserFriendlyException;
            userFriendlyException.ShouldNotBeNull();
        }

        [Fact]
        public async Task Update_ShouldNotThrow_WhenCodeIsDuplicatedButIdIsSame()
        {
            CreateData();
            var ex = await Record.ExceptionAsync(() => _productAppService.UpdateAsync(new ProductDto()
            {
                Code = "Code1",
                Name = "Name",
                Price = 1,
                Id = 1
            }));

            var userFriendlyException = ex as UserFriendlyException;
            userFriendlyException.ShouldBeNull();
        }

        private void CreateData() =>
            UsingDbContext(x => x.Products.AddRange(new Product()
            {
                Code = "Code1",
                Name = "Name1",
                Id = 1,
                Price = 1,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Photo = ""
            }, new Product()
            {
                Code = "Code2",
                Name = "Name2",
                Id = 2,
                Price = 1,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Photo = ""
            }));
    }
}