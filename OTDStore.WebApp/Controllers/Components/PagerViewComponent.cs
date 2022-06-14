using Microsoft.AspNetCore.Mvc;
using OTDStore.ViewModels.Common;
using System.Threading.Tasks;

namespace OTDStore.WebApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
