using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Controllers;
using ProductManager.Products;
using ProductManager.Products.Dto;
using ProductManager.WebHelpers;

namespace ProductManager.Web.Host.Controllers
{
    public class ExportController : ProductManagerControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IDataExport _dataExport;

        public ExportController(IProductAppService productAppService, IDataExport dataExport)
        {
            _productAppService = productAppService;
            _dataExport = dataExport;
        }

        [HttpGet]
        public async Task<IActionResult> ExportProductsToExcel()
        {
            var items = await _productAppService.GetAllAsync(new PagedProductResultRequestDto());
            var bytes = await _dataExport.ExportExcel(items.Items, "sheet");

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Export_{DateTime.Now:g}.xlsx");
        }
    }
}