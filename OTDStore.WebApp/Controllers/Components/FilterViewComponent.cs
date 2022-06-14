using Microsoft.AspNetCore.Mvc;
using OTDStore.ApiIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.WebApp.Controllers.Components
{
    public class FilterViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public FilterViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoryApiClient.GetAll();
            return View(items);
        }
    }
}
