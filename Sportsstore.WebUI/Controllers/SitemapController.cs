using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMvcSitemap;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using Sportsstore.WebUI.Models;

namespace Sportsstore.WebUI.Controllers
{
    public class SitemapController : Controller
    {
        private IProductsRepository repository;


        public SitemapController(IProductsRepository repo)
        {
            repository = repo;
        }

        public ActionResult Index()
        {
            
            List<SitemapNode> nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("HomepageList","Product")),
                new SitemapNode(Url.Action("List","Product"))
                {
                    LastModificationDate = repository.LastModification()
                },            
                new SitemapNode(Url.Action("ListDiscounts", "Product")),

            };
            
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

    }
}
