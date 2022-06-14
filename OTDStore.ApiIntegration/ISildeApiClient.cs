using OTDStore.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.ApiIntegration
{
    public interface ISlideApiClient
    {
        Task<List<SlideVM>> GetAll();
    }
}
