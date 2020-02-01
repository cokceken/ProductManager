using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using ProductManager.Entities;

namespace ProductManager.BackgroundWorkers
{
    public class RemoveUnusedProductPhotos : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Product, int> _productRepository;

        public RemoveUnusedProductPhotos(AbpTimer timer, IRepository<Product, int> productRepository)
            : base(timer)
        {
            _productRepository = productRepository;
            Timer.Period = 60000;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            var photos =
                _productRepository.GetAll()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Photo))
                    .Select(x => x.Photo)
                    .Distinct().ToList();
            var photoNames = photos
                .Select(x => x.Split("/").LastOrDefault())
                .Where(x => x != null)
                .ToList();

            string root = "wwwroot/uploads";
            var files = Directory.GetFiles(root);
            foreach (var file in files)
            {
                var fileName = file.Split("\\").LastOrDefault();
                if (!photoNames.Contains(fileName)) File.Delete(file);
            }
        }
    }
}