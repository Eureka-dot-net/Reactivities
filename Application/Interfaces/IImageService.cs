using Application.Profiles.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult?> UploadImageAsync(IFormFile file);
        Task<string> DeleteImageAsync(string publicId);
    }
}
