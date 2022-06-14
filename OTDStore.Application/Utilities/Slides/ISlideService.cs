using OTDStore.ViewModels.Utilities.Slides;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OTDStore.Application.Utilities.Slides
{
    public interface ISlideService
    {
        Task<List<SlideVM>> GetAll();
    }
}