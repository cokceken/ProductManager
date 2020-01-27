using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ProductManager.Configuration.Dto;

namespace ProductManager.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ProductManagerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
