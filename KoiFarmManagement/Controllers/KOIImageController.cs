using Application.ViewModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace KoiFarmManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class KOIImageController :BaseController
    {
        private readonly Cloudinary _cloudinary;
        private readonly ApiContext _context;
        public KOIImageController(IOptions<Cloud> config, ApiContext context)
        {
            var cloudinaryAccount = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
           );

            _cloudinary = new Cloudinary(cloudinaryAccount);
            _context = context;
        }
        /// <summary>
        /// Upload image product Admin
        /// </summary>
        /// <param name="koiId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("{koiId}/images")]
        public async Task<IActionResult> UploadKoiImages(int koiId, [FromForm] List<IFormFile> files)
        {
            var koi = await _context.Kois.FindAsync(koiId);
            if (koi == null)
                return NotFound("Koi not found");

            var uploadedImageUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Crop("fill").Gravity("face")
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        if (uploadResult.Url != null)
                        {
                            uploadedImageUrls.Add(uploadResult.Url.ToString());

                            var koiImage = new Image
                            {
                                KoiId = koiId,
                                ImageUrl = uploadResult.Url.ToString()
                            };

                            _context.Images.Add(koiImage);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            return Ok(new { imageUrls = uploadedImageUrls });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> cUpdateProductImage(int productId, int imageId, IFormFile file)
        {
            var product = await _context.Kois.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            var productImage =
                await _context.Images.FirstOrDefaultAsync(pi => pi.KoiId == productId && pi.Id == imageId);
            if (productImage == null)
                return NotFound("Product image not found");

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            if (uploadResult.Url == null)
                return BadRequest("Could not upload image");

            // Update the image URL in the database
            productImage.ImageUrl = uploadResult.Url.ToString();
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = productImage.ImageUrl });
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<Image> GetImageInforById(int id)
        {
            return await _context.Images.FindAsync(id);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Image>> GetAllImageInfors()
        {
            return _context.Images.ToList();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task DeleteProductImage(int id)
        {
            var iproduct = await _context.Images.FindAsync(id);
            if (iproduct != null)
            {
                _context.Images.Remove(iproduct);
                await _context.SaveChangesAsync();
            }
        }


    }
}
