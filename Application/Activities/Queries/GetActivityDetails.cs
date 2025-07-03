using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Queries
{
    public class GetActivityDetails
    {
        public record Query(string Id) : IRequest<Domain.Activity> { }

        public class Handler(AppDbContext context) : IRequestHandler<Query, Domain.Activity>
        {
            public async Task<Domain.Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync([request.Id], cancellationToken);
                if (activity == null)
                {
                    throw new Exception("Activity not found");
                }
                return activity;
            }
        }
    }
}
