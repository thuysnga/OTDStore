using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OTDStore.ApiIntegration;
using OTDStore.Utilities.Constants;
using OTDStore.ViewModels.Sales;
using OTDStore.ViewModels.System.Users;
using OTDStore.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IUserApiClient _userApiClient;

        public CartController(IProductApiClient productApiClient, IUserApiClient userApiClient,
            IOrderApiClient orderApiClient)
        {
            _productApiClient = productApiClient;
            _userApiClient = userApiClient;
            _orderApiClient = orderApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        { 
            return View(await GetCheckoutViewModel());
        }
        

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            decimal total = 0;
            var model = await GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailVM>();
            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailVM()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Name = item.Name,
                    Color = item.Color,
                    Memory = item.Memory,
                    RAM = item.RAM

                });
                total += item.Price * item.Quantity;
            }
            var checkoutRequest = new CheckoutRequest()
            {
                UserId = request.CheckoutModel.UserId,
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                Email = request.CheckoutModel.Email,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                Total = total,
                PaymentMethod = request.CheckoutModel.PaymentMethod,
                OrderDetails = orderDetails
            };

            var result1 = await _orderApiClient.CreateOrder(checkoutRequest);
            TempData["SuccessMsg"] = "Đặt hàng thành công";
            return View(model);          
        }

        [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productApiClient.GetById(id);

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            int quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
            }

            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                Image = product.ThumbnailImage,
                Name = product.Name,
                Quantity = quantity,
                Price = product.Price,
                Color = product.Color,
                Memory = product.Memory,
                RAM = product.RAM
            };

            currentCart.Add(cartItem);

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item); 
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        private async Task<CheckoutViewModel> GetCheckoutViewModel()
        {
            var result = await _userApiClient.GetByName(User.Identity.Name);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var checkoutVM = new CheckoutViewModel();
            var user = result.ResultObj;
            checkoutVM = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckoutRequest()
                {
                    Address = user.Address,
                    Email = user.Email,
                    Name = user.LastName + " " + user.FirstName,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.Id
                }
            };
            return checkoutVM;
        }
    }
}
