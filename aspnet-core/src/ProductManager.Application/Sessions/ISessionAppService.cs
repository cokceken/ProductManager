using System.Threading.Tasks;
using Abp.Application.Services;
using ProductManager.Sessions.Dto;

namespace ProductManager.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
