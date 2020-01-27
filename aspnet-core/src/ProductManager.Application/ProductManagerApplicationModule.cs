using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProductManager.Authorization;

namespace ProductManager
{
    [DependsOn(
        typeof(ProductManagerCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ProductManagerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ProductManagerAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ProductManagerApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
