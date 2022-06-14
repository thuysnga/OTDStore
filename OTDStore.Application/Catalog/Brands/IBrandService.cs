using OTDStore.ViewModels.Catalog.Brands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.Application.Catalog.Brands
{
    public interface IBrandService
    {
        Task<List<BrandVM>> GetAll();

        Task<BrandVM> GetByIdb(int id);
    }
}
