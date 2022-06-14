using OTDStore.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVM>> GetAll();
    }
}
