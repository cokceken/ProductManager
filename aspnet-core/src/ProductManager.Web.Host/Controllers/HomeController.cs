using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using ProductManager.Controllers;
using ProductManager.Products;
using ProductManager.Products.Dto;
using ProductManager.WebHelpers;

namespace ProductManager.Web.Host.Controllers
{
    public class HomeController : ProductManagerControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IProductAppService _productAppService;
        private readonly IDataExport _dataExport;

        public HomeController(INotificationPublisher notificationPublisher, IProductAppService productAppService, IDataExport dataExport)
        {
            _notificationPublisher = notificationPublisher;
            _productAppService = productAppService;
            _dataExport = dataExport;
        }

        public IActionResult Index()
        {
            return Redirect("/swagger");
        }

        public async Task<IActionResult> ExportToExcel(PagedProductResultRequestDto input)
        {
            var items = await _productAppService.GetAllAsync(input);
            var bytes = await _dataExport.ExportExcel(items.Items, "sheet");

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Export_{DateTime.Now:g}.xlsx");
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }
    }
}
