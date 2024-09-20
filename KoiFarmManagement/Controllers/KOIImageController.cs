using Application.ViewModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace KoiFarmManagement.Controllers
{
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
        //[Authorize(Roles = "Staff,Admin")]
        [AllowAnonymous]
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
    }
}
