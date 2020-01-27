using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ProductManager.Controllers
{
    public abstract class ProductManagerControllerBase: AbpController
    {
        protected ProductManagerControllerBase()
        {
            LocalizationSourceName = ProductManagerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
