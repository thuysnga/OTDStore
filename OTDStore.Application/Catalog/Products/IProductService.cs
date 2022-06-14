using Microsoft.AspNetCore.Http;
using OTDStore.ViewModels.Catalog.ProductImages;
using OTDStore.ViewModels.Catalog.Products;
using OTDStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<ProductVM> GetById(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addedQuantity);
        Task AddViewCount(int productID);
        Task<PagedResult<ProductVM>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<PagedResult<ProductVM>> GetAllByFilter(GetPublicProductPagingRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ApiResult<bool>> BrandAssign(int id, BrandAssignRequest request);
        Task<List<ProductVM>> GetLatestProducts(int take);
    }
}