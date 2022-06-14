using OTDStore.Data.EF;
using OTDStore.ViewModels.Catalog.Brands;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OTDStore.Application.Catalog.Brands
{
    public class BrandService : IBrandService
    {
        private readonly OTDDbContext _context;

        public BrandService(OTDDbContext context)
        {
            _context = context;
        }

        public async Task<List<BrandVM>> GetAll()
        {
            var query = from b in _context.Brands
                        select new { b };
            return await query.Select(x => new BrandVM()
            {
                Id = x.b.Id,
                Name = x.b.Name,
                ParentId = x.b.ParentId
            }).ToListAsync();
        }

        public async Task<BrandVM> GetByIdb(int id)
        {
            var query = from b in _context.Brands
                        join pib in _context.ProductInBrands on b.Id equals pib.BrandId
                        where pib.ProductId == id
                        select new { b };
            return await query.Select(x => new BrandVM()
            {
                Id = x.b.Id,
                Name = x.b.Name,
                ParentId = x.b.ParentId
            }).FirstOrDefaultAsync();
        }
    }
}
