using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class UserAccessor(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext) : IUserAccessor
    {
        public async Task<User> GetUserAsync()
        {
            return await dbContext.Users.FindAsync(GetUserId())
                   ?? throw new InvalidOperationException("User ID not found.");
        }

        public string GetUserId()
        {
            return httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("No user is logged in.");
                   
        }

        public async Task<User> GetUserWithImagesAsync()
        {
            var userId = GetUserId();
            return await dbContext.Users
                .Include(u => u.Images)
                .FirstOrDefaultAsync(x => x.Id == userId)
                   ?? throw new InvalidOperationException("User ID not found.");
        }
    }
}
