using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public required DbSet<Domain.Activity> Activities { get; set; }

        public required DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        public required DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.ActivityId, a.UserId }));
            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.User)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.Activity)
                .WithMany(x => x.Attendees)
                .HasForeignKey(x => x.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
