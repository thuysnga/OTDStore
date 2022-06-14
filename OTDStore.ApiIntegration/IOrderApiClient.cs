using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<ApiResult<bool>> CreateOrder(CheckoutRequest request);
        Task<ApiResult<PagedResult<OrderVM>>> GetOrdersPagings(GetOrderPagingRequest request);
        Task<ApiResult<PagedResult<OrderVM>>> GetUserOrdersPagings(Guid id, GetOrderPagingRequest request);
        Task<ApiResult<OrderVM>> GetById(int id);
        Task<ApiResult<OrderVM>> GetByUserId(Guid id);
        Task<ApiResult<OrderDetailVM>> GetOrderById(int id);
        Task<ApiResult<bool>> UpdateOrder(int id, StatusUpdateRequest request);
    }
}
