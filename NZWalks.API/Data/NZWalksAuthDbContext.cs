using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "59b48e66-6331-4ae7-9789-1ff66e04dc84";
            var writerRoleId = "9661da11-0311-4f0b-ab66-0f3d0f23423c";
            var roles = new List<IdentityRole>
            {
               new IdentityRole
               {
                    Id = readerRoleId,
                    ConcurrencyStamp =readerRoleId,
                    Name ="Reader",
                    NormalizedName="Reader".ToUpper()
               },
               new IdentityRole
               {
                   Id=writerRoleId,
                   ConcurrencyStamp=writerRoleId,
                   Name= "Writer",
                   NormalizedName="Writer".ToUpper().ToLowerInvariant()
               }

            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
