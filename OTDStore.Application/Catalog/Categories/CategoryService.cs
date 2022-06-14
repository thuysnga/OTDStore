using OTDStore.Data.EF;
using OTDStore.ViewModels.Catalog.Categories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OTDStore.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly OTDDbContext _context;

        public CategoryService(OTDDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryVM>> GetAll()
        {
            var query = from c in _context.Categories
                        select new { c };
            return await query.Select(x => new CategoryVM()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId
            }).ToListAsync();
        }
    }
}
