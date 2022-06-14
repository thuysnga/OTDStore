using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTDStore.Application.System.Users;
using OTDStore.Data.EF;
using OTDStore.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OTDDbContext _context;
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService, OTDDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.Create(request);
            if (result == 0)
            {
                return BadRequest();
            }

            var order = await _orderService.CreateDetail(request, result);
            if (!order)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetOrderPagingRequest request)
        {
            var products = await _orderService.GetOrderPaging(request);
            return Ok(products);
        }

        [HttpGet("{id}/paging")]
        public async Task<IActionResult> GetAllPaging(Guid id, [FromQuery] GetOrderPagingRequest request)
        {
            var products = await _orderService.GetUserOrderPaging(id, request);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetById(id);
            return Ok(order);
        }

        [HttpGet("{id}/list")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            var order = await _orderService.GetByUserId(id);
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StatusUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //[HttpGet("{statistic}")]
        //public async Task<IActionResult> RevenueStatistic(TimeRequest request)
        //{
        //    var order = await _orderService.RevenueStatistic(request);
        //    return Ok(order);
        //}
        
    }
}
