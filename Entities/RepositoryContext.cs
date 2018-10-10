using Microsoft.EntityFrameworkCore;
using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Collection> Collections { get; set; }
    }
}