using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Commands
{
    public class DeleteImage
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string ImageId { get; set; }
        }

        public class Handler(IImageService imageService,
            IUserAccessor userAccessor, AppDbContext dbContext) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userAccessor.GetUserWithImagesAsync();
                var image = user.Images.FirstOrDefault(x => x.Id == request.ImageId);

                if (image == null)
                {
                    return Result<Unit>.Failure("Image not found.", 404);
                }
                if (image.Url == user.ImageUrl)
                {
                    return Result<Unit>.Failure("Cannot delete the main image.", 400);
                }

                await imageService.DeleteImageAsync(image.PublicId);

                user.Images.Remove(image);

                var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Failed to delete image.", 500);
            }
        }
    }
}
