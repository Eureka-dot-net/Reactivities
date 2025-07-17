﻿using Application.Core;
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

namespace Application.Profiles.Commands
{
    public class GetUserProfile
    {
        public class Query : IRequest<Result<UserProfile>>
        {
            public required string UserId { get; set; }
        }

        public class Handler( AppDbContext dbContext, IMapper mapper) : IRequestHandler<Query, Result<UserProfile>>
        {
            public async Task<Result<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await dbContext.Users
                    .ProjectTo<UserProfile>(mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
                return profile == null
                    ? Result<UserProfile>.Failure("User profile not found.", 404)
                    : Result<UserProfile>.Success(profile);
            }
        }
    }
}
