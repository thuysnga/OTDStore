using OTDStore.ViewModels.Catalog.Products;
using OTDStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVM>> GetPagings(GetManageProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<bool> UpdateProduct(ProductUpdateRequest request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ApiResult<bool>> BrandAssign(int id, BrandAssignRequest request);

        Task<ProductVM> GetById(int id);

        Task<ProductDetailVM> GetByIdApp(int id);

        Task<List<ProductVM>> GetLatestProducts(int take);

        Task<PagedResult<ProductVM>> GetAllPagings(GetPublicProductPagingRequest request);

        Task<bool> DeleteProduct(int id);
    }
}
