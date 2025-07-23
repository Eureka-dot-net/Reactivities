using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Commands
{
    public class FollowToggle
    {
        public record Command(string TargetUserId) : IRequest<Result<Unit>>;

        public class Handler(IUserAccessor userAccessor, AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await userAccessor.GetUserAsync();
                var target = await context.Users.FindAsync([request.TargetUserId],cancellationToken);
                if (target == null)
                {
                    return Result<Unit>.Failure("Target user not found", 400);
                }
                var following = await context.UserFollowings
                    .FindAsync([observer.Id, target.Id], cancellationToken);
                if (following == null)
                {
                    context.UserFollowings.Add(new UserFollowing
                    {
                        ObserverId = observer.Id,
                        TargetId = target.Id
                    });
                } else
                {
                    context.UserFollowings.Remove(following);
                }

                return await context.SaveChangesAsync(cancellationToken) > 0
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Problem toggling follow status", 500);
            }
        }
    }
}
