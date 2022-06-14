using OTDStore.Data.EF;
using OTDStore.Data.Entities;
using OTDStore.Utilities.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTDStore.ViewModels.Catalog.Products;
using OTDStore.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using OTDStore.Application.Common;
using OTDStore.ViewModels.Catalog.ProductImages;

namespace OTDStore.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly OTDDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public ProductService(OTDDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Insurance = request.Insurance,
                Description = request.Description,
                YearRelease = request.YearRelease,
                Color = request.Color,
                CPU = request.CPU,
                VGA = request.VGA,
                Memory = request.Memory,
                RAM = request.RAM,
                Camera = request.Camera,
                Bluetooth = request.Bluetooth,
                Monitor = request.Monitor,
                Battery = request.Battery,
                Size = request.Size,
                OS = request.OS,
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
            };

            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null) throw new OTDStoreException($"Không thể tìm thấy sản phẩm có ID: {request.Id}");

            product.Name = request.Name;
            product.Insurance = request.Insurance;
            product.Description = request.Description;
            product.YearRelease = request.YearRelease;
            product.Color = request.Color;
            product.CPU = request.CPU;
            product.VGA = request.VGA;
            product.Memory = request.Memory;
            product.RAM = request.RAM;
            product.Camera = request.Camera;
            product.Bluetooth = request.Bluetooth;
            product.Monitor = request.Monitor;
            product.Battery = request.Battery;
            product.Size = request.Size;
            product.OS = request.OS;
            product.Price = request.Price;
            product.OriginalPrice = request.OriginalPrice;
            product.Stock = request.Stock;

            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new OTDStoreException($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductVM>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pib in _context.ProductInBrands on p.Id equals pib.ProductId into ppib
                        from pib in ppib.DefaultIfEmpty()
                        join b in _context.Brands on pib.BrandId equals b.Id into pibb
                        from b in pibb.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        select new { p, pic, pib, pi, b, c };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            if (request.BrandId != null && request.BrandId != 0)
            {
                query = query.Where(p => p.pib.BrandId == request.BrandId);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    Insurance = x.p.Insurance,
                    Description = x.p.Description,
                    YearRelease = x.p.YearRelease,
                    Color = x.p.Color,
                    CPU = x.p.CPU,
                    VGA = x.p.VGA,
                    Memory = x.p.Memory,
                    RAM = x.p.RAM,
                    Camera = x.p.Camera,
                    Bluetooth = x.p.Bluetooth,
                    Monitor = x.p.Monitor,
                    Battery = x.p.Battery,
                    Size = x.p.Size,
                    OS = x.p.OS,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ProductVM> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            var categories = await (from c in _context.Categories
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId
                                    select c.Name).ToListAsync();
            var brands = await (from b in _context.Brands
                                join pib in _context.ProductInBrands on b.Id equals pib.BrandId
                                where pib.ProductId == productId
                                select b.Name).ToListAsync();

            var image = await _context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true).FirstOrDefaultAsync();

            var productViewModel = new ProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Insurance = product.Insurance,
                Description = product.Description,
                YearRelease = product.YearRelease,
                Color = product.Color,
                CPU = product.CPU,
                VGA = product.VGA,
                Memory = product.Memory,
                RAM = product.RAM,
                Camera = product.Camera,
                Bluetooth = product.Bluetooth,
                Monitor = product.Monitor,
                Battery = product.Battery,
                Size = product.Size,
                OS = product.OS,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Stock = product.Stock,
                DateCreated = product.DateCreated,
                ThumbnailImage = image != null ? image.ImagePath : "no-image.jpg",
                Brands = brands,
                Categories = categories,
            };
            return productViewModel;
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new OTDStoreException($"Không thể tìm thấy sản phẩm có ID: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new OTDStoreException($"Không thể tìm thấy sản phẩm có ID: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new OTDStoreException($"Không tìm thấy ảnh có ID {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new OTDStoreException($"Không tìm thấy ảnh có ID {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new OTDStoreException($"Không tìm thấy ảnh có ID {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<PagedResult<ProductVM>> GetAllByFilter(GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pib in _context.ProductInBrands on p.Id equals pib.ProductId into ppib
                        from pib in ppib.DefaultIfEmpty()
                        join b in _context.Brands on pib.BrandId equals b.Id into pibb
                        from b in pibb.DefaultIfEmpty()
                        select new { p, pic, pib };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            if (request.BrandId != null && request.BrandId != 0)
            {
                query = query.Where(p => p.pib.BrandId == request.BrandId);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    Insurance = x.p.Insurance,
                    Description = x.p.Description,
                    YearRelease = x.p.YearRelease,
                    Color = x.p.Color,
                    CPU = x.p.CPU,
                    VGA = x.p.VGA,
                    Memory = x.p.Memory,
                    RAM = x.p.RAM,
                    Camera = x.p.Camera,
                    Bluetooth = x.p.Bluetooth,
                    Monitor = x.p.Monitor,
                    Battery = x.p.Battery,
                    Size = x.p.Size,
                    OS = x.p.OS,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
            }
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> BrandAssign(int id, BrandAssignRequest request)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
            }
            foreach (var brand in request.Brands)
            {
                var productInBrand = await _context.ProductInBrands
                    .FirstOrDefaultAsync(x => x.BrandId == int.Parse(brand.Id)
                    && x.ProductId == id);
                if (productInBrand != null && brand.Selected == false)
                {
                    _context.ProductInBrands.Remove(productInBrand);
                }
                else if (productInBrand == null && brand.Selected)
                {
                    await _context.ProductInBrands.AddAsync(new ProductInBrand()
                    {
                        BrandId = int.Parse(brand.Id),
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductVM>> GetLatestProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pi == null || pi.IsDefault == true
                        select new { p, pic, pi };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            return data;
        }
    }
}