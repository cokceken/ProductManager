using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.UI;
using ProductManager.Authorization.Users;
using ProductManager.Entities;
using ProductManager.Products.Dto;

namespace ProductManager.Products
{
    public class ProductAppService :
        AsyncCrudAppService<Product, ProductDto, int, PagedProductResultRequestDto, CreateProductDto, ProductDto>,
        IProductAppService
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public ProductAppService(IRepository<Product, int> repository, INotificationPublisher notificationPublisher,
            INotificationSubscriptionManager notificationSubscriptionManager) :
            base(repository)
        {
            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
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

            var item = await base.CreateAsync(input);
            await PublishNotification(item);
            
            return item;
        }

        public override async Task<ProductDto> UpdateAsync(ProductDto input)
        {
            if (CheckDuplicateProductCode(input.Code, input.Id))
                throw new UserFriendlyException(L("ActionError"), L("ProductCodeDuplicate"));

            var item = await base.UpdateAsync(input);
            await PublishNotification(item);
            
            return item;
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            await base.DeleteAsync(input);
            await PublishNotificationDelete(input);
        }

        private async Task PublishNotificationDelete(EntityDto<int> item)
        {
            await ManageSubscription("ProductData.Delete");

            await _notificationPublisher.PublishAsync("ProductData.Delete", new NotificationData()
            {
                Properties = new Dictionary<string, object>()
                {
                    {"id", item.Id }
                }
            });
        }

        private bool CheckDuplicateProductCode(string productCode, int id = -1)
        {
            return Repository.GetAll().Any(x => x.Code == productCode && x.Id != id && x.IsDeleted == false);
        }

        //Just for test, could be implemented better
        private async Task ManageSubscription(string notificationName)
        {
            var userIdentifier = new UserIdentifier(AbpSession.TenantId, AbpSession.UserId ?? -1);
            if (AbpSession.UserId.HasValue &&
                !_notificationSubscriptionManager.IsSubscribed(
                    userIdentifier, notificationName))
            {
                await _notificationSubscriptionManager.SubscribeAsync(userIdentifier, notificationName);
            }
        }

        private async Task PublishNotification(ProductDto item)
        {
            await ManageSubscription("ProductData.New");

            //await _notificationPublisher.PublishAsync("ProductData.New",
            //    new ProductNotificationData(item.Code, item.Name, item.Photo, item.Price, item.IsDeleted,
            //        item.CreationTime, item.CreatorUserId, item.DeleterUserId, item.DeletionTime,
            //        item.LastModifierUserId, item.LastModificationTime));

            await _notificationPublisher.PublishAsync("ProductData.New", new NotificationData()
            {
                Properties = new Dictionary<string, object>()
                {
                    {"id", item.Id },
                    {"code", item.Code},
                    {"name", item.Name},
                    {"photo", item.Photo},
                    {"price", item.Price},
                    {"isDeleted", item.IsDeleted},
                    {"creationTime", item.CreationTime},
                    {"creatorUserId", item.CreatorUserId},
                    {"deleterUserId", item.DeleterUserId},
                    {"deletionTime", item.DeletionTime},
                    {"lastModifierUserId", item.LastModifierUserId},
                    {"lastModificationTime", item.LastModificationTime}
                }
            });
        }
    }
}