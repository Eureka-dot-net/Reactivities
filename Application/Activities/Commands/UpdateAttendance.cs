using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Commands;

public class UpdateAttendance
{
    public record Command(string ActivityId, bool IsGoing) : IRequest<Result<Unit>>;


    public class Handler(AppDbContext context, IUserAccessor userAccessor) : 
        IRequestHandler<Command, Result<Unit>>
    {
        private static bool UserIsHost(Activity activity, string userId) =>
            activity.Attendees.Any(a => a.UserId == userId && a.IsHost);
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .Include(a => a.Attendees) // Needed to access attendee data
                .FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);

            if (activity is null)
                return Result<Unit>.Failure("Activity not found", 404);

            var user = await userAccessor.GetUserAsync();

            if (user is null)
                return Result<Unit>.Failure("User not found", 404);

            var host = activity.Attendees.FirstOrDefault(a => a.IsHost);

            if (UserIsHost(activity, user.Id))
            {
                activity.IsCancelled = !request.IsGoing;
            }
            else
            {
                var existingAttendee = activity.Attendees.FirstOrDefault(a => a.UserId == user.Id);

                if (existingAttendee is not null)
                {
                    if (request.IsGoing)
                        return Result<Unit>.Failure("You are already attending this activity", 400);

                    activity.Attendees.Remove(existingAttendee);
                }
                else if (request.IsGoing)
                {
                    activity.Attendees.Add(new ActivityAttendee
                    {
                        ActivityId = activity.Id,
                        UserId = user.Id,
                        User = user,
                        Activity = activity,
                        IsHost = false
                    });
                }
            }

            var success = await context.SaveChangesAsync(cancellationToken) > 0;

            return success
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Failed to update attendance", 400);
        }
    }
}
