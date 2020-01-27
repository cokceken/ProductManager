using Abp.Domain.Entities.Auditing;

namespace ProductManager.Entities
{
    public class Product : FullAuditedEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
    }
}