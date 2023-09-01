using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class CSGODbContext : DbContext
    {
        public CSGODbContext(DbContextOptions<CSGODbContext> options)
            : base(options)
        {
        }

        public DbSet<CSGOEntity> CSGOItems { get; set; } = null!;
    }
}
