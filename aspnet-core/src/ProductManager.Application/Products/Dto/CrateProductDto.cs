using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using ProductManager.Entities;

namespace ProductManager.Products.Dto
{
    [AutoMapTo(typeof(Product))]
    public class CrateProductDto : IShouldNormalize
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        public string Photo { get; set; }

        [Required]
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$")]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
        public void Normalize()
        {

        }
    }
}