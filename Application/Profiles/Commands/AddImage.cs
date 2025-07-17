using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Commands
{
    public class AddImage
    {
        public class Command : IRequest<Result<Image>>
        {
            public required IFormFile File { get; set; }
        }

        public class Handler(IUserAccessor userAccessor, AppDbContext dbContext, IImageService imageService) 
            : IRequestHandler<Command, Result<Image>>
        {
            public async Task<Result<Image>> Handle(Command request, CancellationToken cancellationToken)
            {
                var uploadResult = await imageService.UploadImageAsync(request.File);

                if (uploadResult == null)
                {
                    return Result<Image>.Failure("Image upload failed", 400);
                }

                var user = await userAccessor.GetUserAsync();

                var image = new Image
                {
                    Url = uploadResult.Url,
                    PublicId = uploadResult.PublicId,
                    UserId = user.Id
                };

                user.ImageUrl ??= uploadResult.Url;

                dbContext.Images.Add(image);

                var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Image>.Success(image)
                    : Result<Image>.Failure("Problem saving image", 500);
            }
        }
    }
}
