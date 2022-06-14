using OTDStore.Data.EF;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTDStore.ViewModels.Utilities.Slides;

namespace OTDStore.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly OTDDbContext _context;

        public SlideService(OTDDbContext context)
        {
            _context = context;
        }

        public async Task<List<SlideVM>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideVM()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Url = x.Url,
                    Image = x.Image
                }).ToListAsync();

            return slides;
        }
    }
}