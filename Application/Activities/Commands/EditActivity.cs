using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Commands
{
    public class EditActivity
    {
        public class Command : IRequest
        {
            public required Domain.Activity Activity { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command>
        {
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var existingActivity = await context.Activities.FindAsync([request.Activity.Id], cancellationToken) 
                    ?? throw new Exception("Activity not found");
                mapper.Map(request.Activity, existingActivity);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
