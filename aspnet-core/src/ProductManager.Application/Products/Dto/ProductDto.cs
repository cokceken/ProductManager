using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProductManager.Entities;

namespace ProductManager.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class ProductDto : EntityDto
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        public string Photo { get; set; }

        [Required]
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$")]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
    }
}