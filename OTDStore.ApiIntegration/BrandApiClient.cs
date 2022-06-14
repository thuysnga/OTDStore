using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OTDStore.ViewModels.Catalog.Brands;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public class BrandApiClient : BaseApiClient, IBrandApiClient
    {
        public BrandApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<BrandVM>> GetAll()
        {
            return await GetListAsync<BrandVM>("/api/brands");
        }

        public async Task<BrandVM> GetByIdb(int id)
        {
            return await GetAsync<BrandVM>($"/api/brands/{id}");
        }
    }
}
