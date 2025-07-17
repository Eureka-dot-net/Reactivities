using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles.Queries
{
    public class GetProfileImages
    {
        public class Query : IRequest<Result<List<Image>>>
        {
            public required string UserId { get; set; } = string.Empty;
        }

        public class Handler(AppDbContext dbContext) : IRequestHandler<Query, Result<List<Image>>>
        {
            public async Task<Result<List<Image>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var images = await dbContext.Images
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync(cancellationToken);
                //if (!images.Any())
                //{
                //    return Result<List<Image>>.Failure("No images found for this user", 404);
                //}
                return Result<List<Image>>.Success(images);
            }
        }
    }
}
