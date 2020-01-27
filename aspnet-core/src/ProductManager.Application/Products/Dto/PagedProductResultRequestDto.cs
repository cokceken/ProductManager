using Abp.Application.Services.Dto;

namespace ProductManager.Products.Dto
{
    public class PagedProductResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}