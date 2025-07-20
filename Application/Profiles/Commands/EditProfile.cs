using Application.Core;
using Application.Interfaces;
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
    public class EditProfile
    {
        public record Command(string DisplayName, string? Bio) : IRequest<Result<Unit>>{

        }

        public class Handler(IUserAccessor userAccessor, AppDbContext context) 
            : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userAccessor.GetUserAsync();
                if (user == null)
                {
                    return Result<Unit>.Failure("User not found.", StatusCodes.Status404NotFound);
                }
                user.DisplayName = request.DisplayName.Trim();
                user.Bio = request.Bio;

                var result = await context.SaveChangesAsync() > 0;
                return result ?
                    Result<Unit>.Success(Unit.Value) :
                    Result<Unit>.Failure("Failed to update profile.", StatusCodes.Status500InternalServerError);
            }
        }
    }
}
