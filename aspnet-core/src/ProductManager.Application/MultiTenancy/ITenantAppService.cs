using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ProductManager.MultiTenancy.Dto;

namespace ProductManager.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

