using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProductManager.Web.Host.Dto
{
    public class CreateImageRequest
    {
        [Required] public IFormFile File { get; set; }
    }
}