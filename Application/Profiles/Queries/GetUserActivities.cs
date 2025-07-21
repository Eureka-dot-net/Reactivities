using Application.Activities.DTO;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries
{
    public class GetUserActivities
    {

        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string UserId { get; set; } = string.Empty;
            public string Filter { get; set; } = string.Empty;

        }

        public class Handler(AppDbContext context, IMapper mapper) :
            IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            public async Task<Result<List<UserActivityDto>>>
                Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Activities
                    .OrderBy(x => x.Date)
                    .Where(x => x.Attendees.Any(a => a.UserId == request.UserId))
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Filter))
                {
                    query = request.Filter.ToLower() switch
                    {
                        "future" => query.Where(x =>
                            x.Date >= DateTime.UtcNow),
                        "past" => query.Where(x =>
                            x.Date < DateTime.UtcNow),
                        "hosting" => query.Where(x =>
                            x.Attendees.Any(a => a.IsHost && (a.UserId == request.UserId))),
                        _ => query
                    };
                }

                var projectedActivities = query.ProjectTo<UserActivityDto>(mapper.ConfigurationProvider);

                var activities = await projectedActivities
                    .ToListAsync(cancellationToken);

                return Result<List<UserActivityDto>>.Success(activities);
            }
        }
    }
}
