using OTDStore.ViewModels.Catalog.Brands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public interface IBrandApiClient
    {
        Task<List<BrandVM>> GetAll();

        Task<BrandVM> GetByIdb(int id);
    }
}
