using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using Sportsstore.WebUI.Models;

namespace Sportsstore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        private IProductsRepository repository;
        public int PageSize = 4;

        public ProductController(IProductsRepository productRepository)
        {
            this.repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        repository.Products.Count() :
                        repository.Products.Where( e => e.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public ViewResult ListDiscounts(int nrProducts = 5)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(p => p.Discount != false)
                .Take(nrProducts)
            };

            return View(model); 
        }

        public ViewResult HomepageList(int nrProducts = 3)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .OrderBy(p => Guid.NewGuid())
                .Take(nrProducts)
            };
            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public FileResult GetDefaultImg()
        {
            string fileName = @"..\Content\assets\img\no-thumbnail.png";
            string contentType = "image/png";
            
            return new FilePathResult(fileName, contentType);
        }
        
    }
}
