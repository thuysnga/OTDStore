using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OTDStore.ViewModels.Utilities.Slides;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<SlideVM>> GetAll()
        {
            return await GetListAsync<SlideVM>("/api/slides");
        }
    }
}
