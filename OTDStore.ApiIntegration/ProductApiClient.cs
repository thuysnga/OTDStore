using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OTDStore.ApiIntegration;
using OTDStore.Utilities.Constants;
using OTDStore.ViewModels.Catalog.Products;
using OTDStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagedResult<ProductVM>> GetPagings(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ProductVM>>(
                $"/api/products/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}&brandId={request.BrandId}");

            return data;
        }

        public async Task<PagedResult<ProductVM>> GetAllPagings(GetPublicProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ProductVM>>(
                $"/api/products/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}&keyword={request.Keyword}" +
                $"&categoryId={request.CategoryId}&brandId={request.BrandId}");

            return data;
        }

        public async Task<bool> CreateProduct(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.YearRelease.ToString()), "yearRelease");
            requestContent.Add(new StringContent(request.Insurance.ToString()), "insurance");
            requestContent.Add(new StringContent(request.Color.ToString()), "color");
            requestContent.Add(new StringContent(request.CPU.ToString()), "cpu");
            requestContent.Add(new StringContent(request.VGA.ToString()), "vga");
            requestContent.Add(new StringContent(request.Memory.ToString()), "memory");
            requestContent.Add(new StringContent(request.RAM.ToString()), "ram");
            requestContent.Add(new StringContent(request.Camera.ToString()), "camera");
            requestContent.Add(new StringContent(request.Bluetooth.ToString()), "bluetooth");
            requestContent.Add(new StringContent(request.Monitor.ToString()), "monitor");
            requestContent.Add(new StringContent(request.Battery.ToString()), "battery");
            requestContent.Add(new StringContent(request.Size.ToString()), "size");
            requestContent.Add(new StringContent(request.OS.ToString()), "os");
            requestContent.Add(new StringContent(request.DateCreated.ToString()), "datecreate");

            var response = await client.PostAsync($"/api/products/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProduct(ProductUpdateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.YearRelease.ToString()), "yearRelease");
            requestContent.Add(new StringContent(request.Insurance.ToString()), "insurance");
            requestContent.Add(new StringContent(request.Color.ToString()), "color");
            requestContent.Add(new StringContent(request.CPU.ToString()), "cpu");
            requestContent.Add(new StringContent(request.VGA.ToString()), "vga");
            requestContent.Add(new StringContent(request.Memory.ToString()), "memory");
            requestContent.Add(new StringContent(request.RAM.ToString()), "ram");
            requestContent.Add(new StringContent(request.Camera.ToString()), "camera");
            requestContent.Add(new StringContent(request.Bluetooth.ToString()), "bluetooth");
            requestContent.Add(new StringContent(request.Monitor.ToString()), "monitor");
            requestContent.Add(new StringContent(request.Battery.ToString()), "battery");
            requestContent.Add(new StringContent(request.Size.ToString()), "size");
            requestContent.Add(new StringContent(request.OS.ToString()), "os");

            var response = await client.PutAsync($"/api/products/" + request.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/products/{id}/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> BrandAssign(int id, BrandAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/products/{id}/brands", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ProductVM> GetById(int id)
        {
            var data = await GetAsync<ProductVM>($"/api/products/{id}");
            return data;
        }

        public async Task<ProductDetailVM> GetByIdApp(int id)
        {
            var data = await GetAsync<ProductDetailVM>($"/api/products/detail/{id}");

            return data;
        }

        public async Task<List<ProductVM>> GetLatestProducts(int take)
        {
            var data = await GetListAsync<ProductVM>($"/api/products/latest/{take}");
            return data;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await Delete($"/api/products/" + id);
        }
    }
}
