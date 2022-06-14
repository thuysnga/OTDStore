using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OTDStore.Data.EF;
using OTDStore.Data.Entities;
using OTDStore.Data.Enum;
using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.Application.System.Users
{
    public class OrderService : IOrderService
    {
        private readonly OTDDbContext _context;
        private readonly IConfiguration _config;
        public OrderService(IConfiguration config, OTDDbContext context)
        {
            _context = context;
            _config = config;
        }

        public async Task<ApiResult<PagedResult<OrderVM>>> GetOrderPaging(GetOrderPagingRequest request)
        {
            var query = from o in _context.Orders
                        select new { o };

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderVM()
                {
                    Id = x.o.Id,
                    ShipName = x.o.ShipName,
                    ShipAddress = x.o.ShipAddress,
                    ShipEmail = x.o.ShipEmail,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,
                    Total = x.o.Total,
                    PaymentMethod = x.o.PaymentMethod,
                    Status = x.o.Status,
                    OrderDate = x.o.OrderDate
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<OrderVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<OrderVM>>(pagedResult);
        }

        public async Task<int> Create(CheckoutRequest request)
        {
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = request.UserId,
                ShipName = request.Name,
                ShipAddress = request.Address,
                ShipEmail = request.Email,
                ShipPhoneNumber = request.PhoneNumber,
                Total = request.Total,
                PaymentMethod = request.PaymentMethod,
                Status = (OrderStatus)0,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<bool> CreateDetail(CheckoutRequest request, int id)
        {
            foreach (var item in request.OrderDetails)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Name = item.Name,
                    Color = item.Color,
                    Memory = item.Memory,
                    RAM = item.RAM
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApiResult<OrderVM>> GetById(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return new ApiErrorResult<OrderVM>("Không tồn tại hóa đơn");
            }

            var query = from od in _context.OrderDetails
                        where od.OrderId == id
                        select new { od };

            var data = await query.Select(x => new OrderDetailVM()
            {
                ProductId = x.od.ProductId,
                Quantity = x.od.Quantity,
                Name = x.od.Name,
                Memory = x.od.Memory,
                Color = x.od.Color,
                RAM = x.od.RAM,
                Price = x.od.Price,
            }).ToListAsync();

            var orderViewModel = new OrderVM()
            {
                Id = id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipPhoneNumber = order.ShipPhoneNumber,
                Total = order.Total,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                OrderDetails = data
            };
            return new ApiSuccessResult<OrderVM>(orderViewModel);
        }

        public async Task<List<OrderVM>> GetByUserId(Guid id)
        {
            var order = from o in _context.Orders
                        where o.UserId == id
                        select o;

            var data = await order.OrderByDescending(x => x.OrderDate)
                .Select(x => new OrderVM()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    ShipName = x.ShipName,
                    ShipAddress = x.ShipAddress,
                    ShipEmail = x.ShipEmail,
                    ShipPhoneNumber = x.ShipPhoneNumber,
                    Total = x.Total,
                    PaymentMethod = x.PaymentMethod,
                    Status = x.Status,
                }).ToListAsync();
            return data;
        }

        public async Task<ApiResult<PagedResult<OrderVM>>> GetUserOrderPaging(Guid id, GetOrderPagingRequest request)
        {
            var query = from o in _context.Orders
                        where o.UserId == id
                        orderby o.OrderDate descending
                        select new { o };

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderVM()
                {
                    Id = x.o.Id,
                    ShipName = x.o.ShipName,
                    ShipAddress = x.o.ShipAddress,
                    ShipEmail = x.o.ShipEmail,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,
                    Total = x.o.Total,
                    PaymentMethod = x.o.PaymentMethod,
                    Status = x.o.Status,
                    OrderDate = x.o.OrderDate
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<OrderVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<OrderVM>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Update(int id, StatusUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            int i = (int)order.Status;
            int j = (int)request.Status;
            order.Status = request.Status;

            if ((j != 0 || j != 4) && i == 0)
            {
                var query = from od in _context.OrderDetails
                            where od.OrderId == id
                            select new { od };

                var data = await query.Select(x => new OrderDetailVM()
                {
                    ProductId = x.od.ProductId,
                    Quantity = x.od.Quantity,
                }).ToListAsync();

                foreach (var item in data)
                {
                    var product = (from p in _context.Products
                                   where p.Id == item.ProductId
                                   select p).FirstOrDefault();
                    if (item.Quantity > product.Stock)
                    {
                        return new ApiErrorResult<bool>($"Số lượng sản phẩm có id = {item.ProductId} không đủ.");
                    }
                    else
                    {
                        product.Stock = product.Stock - item.Quantity;
                        _context.Products.Update(product);
                    }
                }
            }
            _context.Orders.Update(order);
            var result = await _context.SaveChangesAsync();
            if (result != 0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

    }
}