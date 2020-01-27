using System.Threading.Tasks;
using ProductManager.Configuration.Dto;

namespace ProductManager.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
