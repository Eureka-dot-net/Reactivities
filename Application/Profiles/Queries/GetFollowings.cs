using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
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

namespace Application.Profiles.Queries
{
    public class GetFollowings
    {
        public record Query(string userId, string Predicate = "followers") : IRequest<Result<List<UserProfile>>>;

        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<List<UserProfile>>>
        {
            public async Task<Result<List<UserProfile>>> Handle(Query request, CancellationToken cancellationToken)
            {
               var profiles = new List<UserProfile>();
                switch(request.Predicate.ToLower())
                {
                    case "followers":
                        profiles = await context.UserFollowings.Where(x => x.TargetId == request.userId)
                            .Select(x => x.Observer)
                            .ProjectTo<UserProfile>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId()})
                            .ToListAsync(cancellationToken);
                        break;
                    case "followings":
                        profiles = await context.UserFollowings.Where(x => x.ObserverId == request.userId)
                            .Select(x => x.Target)
                            .ProjectTo<UserProfile>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId() })
                            .ToListAsync(cancellationToken);
                        break;
                }
                return Result<List<UserProfile>>.Success(profiles);
            }
        }
    }
}
