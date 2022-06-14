using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTDStore.Application.Catalog.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _brandService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdb(int id)
        {
            var brand = await _brandService.GetByIdb(id);
            return Ok(brand);
        }
    }
}
