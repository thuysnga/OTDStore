using Microsoft.AspNetCore.Mvc;
using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OTDStore.Application.System.Users
{
    public interface IOrderService
    {
        Task<int> Create(CheckoutRequest request);

        Task<bool> CreateDetail(CheckoutRequest request, int id);

        Task<ApiResult<PagedResult<OrderVM>>> GetOrderPaging(GetOrderPagingRequest request);

        Task<ApiResult<PagedResult<OrderVM>>> GetUserOrderPaging(Guid id, GetOrderPagingRequest request);

        Task<ApiResult<OrderVM>> GetById(int id);

        //Task<IActionResult> RevenueStatistic(TimeRequest request);

        Task<List<OrderVM>> GetByUserId(Guid id);

        Task<ApiResult<bool>> Update(int id, StatusUpdateRequest request);
    }
}