using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Controllers;
using ProductManager.Web.Host.Dto;

namespace ProductManager.Web.Host.Controllers
{
    public class ImageController : ProductManagerControllerBase
    {
        public static IWebHostEnvironment Environment;

        public ImageController(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        [HttpPost]
        public async Task<CreateImageResponse> CreateImage(CreateImageRequest request)
        {
            if (!Directory.Exists(Environment.WebRootPath + "\\uploads\\"))
            {
                Directory.CreateDirectory(Environment.WebRootPath + "\\uploads\\");
            }

            var extension = MimeTypes.MimeTypeMap.GetExtension(request.File.ContentType);
            string fileName;
            do
            {
                var fileId = Guid.NewGuid();
                fileName = $"{fileId}{extension}";
            } while (System.IO.File.Exists(fileName));
            
            using (var fileStream =
                System.IO.File.Create(Environment.WebRootPath + "\\uploads\\" + fileName))
            {
                request.File.CopyTo(fileStream);
                fileStream.Flush();
                return new CreateImageResponse()
                {
                    ImageId = fileName
                };
            }
        }
    }
}