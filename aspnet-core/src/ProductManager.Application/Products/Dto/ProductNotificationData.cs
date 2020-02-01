using System;
using Abp.Notifications;

namespace ProductManager.Products.Dto
{
    [Serializable]
    public class ProductNotificationData : NotificationData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public ProductNotificationData(string code, string name, string photo, decimal price, bool isDeleted,
            DateTime creationTime, long? creatorUserId, long? deleterUserId, DateTime? deletionTime,
            long? lastModifierUserId, DateTime? lastModificationTime)
        {
            Code = code;
            Name = name;
            Photo = photo;
            Price = price;
            IsDeleted = isDeleted;
            CreationTime = creationTime;
            CreatorUserId = creatorUserId;
            DeleterUserId = deleterUserId;
            DeletionTime = deletionTime;
            LastModifierUserId = lastModifierUserId;
            LastModificationTime = lastModificationTime;
        }
    }
}