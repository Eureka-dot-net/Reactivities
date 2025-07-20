using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Queries
{
    public class GetComments
    {
        public record Query(string ActivityId) : IRequest<Result<List<CommentDto>>>;

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await context.Comments
                    .Where(c => c.ActivityId == request.ActivityId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                //if (comments == null || !comments.Any())
                //{
                //    return Result<List<CommentDto>>.Failure("No comments found for this activity", 404);
                //}
                return Result<List<CommentDto>>.Success(comments);
            }
        }
    }
}
