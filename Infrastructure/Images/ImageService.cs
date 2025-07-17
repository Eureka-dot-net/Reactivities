using Application.Interfaces;
using Application.Profiles.DTOs;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Images
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(IOptions<CloudinarySetttings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<Application.Profiles.DTOs.ImageUploadResult?> UploadImageAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "reactivities",
                    // Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.Error != null)
                {
                    throw new Exception(result.Error.Message);
                }
                return new Application.Profiles.DTOs.ImageUploadResult
                {
                    PublicId = result.PublicId,
                    Url = result.SecureUrl.ToString()
                };
            }

            return null; // or throw, depending on your design
        }


        public async Task<string> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }

            return result.Result;
        }
    }
}
