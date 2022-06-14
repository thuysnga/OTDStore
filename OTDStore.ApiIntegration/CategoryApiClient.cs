using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OTDStore.ApiIntegration;
using OTDStore.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<CategoryVM>> GetAll()
        {
            return await GetListAsync<CategoryVM>("/api/categories");
        }
    }
}
