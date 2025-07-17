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
    public class SetMainImage
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string ImageId { get; set; }
        }

        public class Handler(
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
                
                user.ImageUrl = image.Url;

                var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Failed to set main image.", 500);
            }
        }
    }
}
