using OTDStore.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAll();
    }
}
