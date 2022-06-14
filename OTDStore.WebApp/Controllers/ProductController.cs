using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OTDStore.ApiIntegration;
using OTDStore.Utilities.Constants;
using OTDStore.ViewModels.Catalog.Products;
using OTDStore.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IBrandApiClient _brandApiClient;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration,
            ICategoryApiClient categoryApiClient, IBrandApiClient brandApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _brandApiClient = brandApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productApiClient.GetById(id);
            var brand = await _brandApiClient.GetByIdb(id);
            return View(new ProductDetailViewModel()
            {
                Product = product,
                Brand = brand
            }) ;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int? brandId, int page = 1, int pageSize = 8)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = page,
                PageSize = pageSize,
                CategoryId = categoryId,
                BrandId = brandId
            };
            var data = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            var brands = await _brandApiClient.GetAll();
            ViewBag.Brands = brands.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = brandId.HasValue && brandId.Value == x.Id
            });

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
    }
}
