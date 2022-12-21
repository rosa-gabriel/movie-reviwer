using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext() { }
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<CastEntry> CastEntries { get; set; }
        public DbSet<CastType> CastTypes { get; set; }
        public DbSet<FavoriteEntry> FavoriteEntries { get; set; }
        public DbSet<TagName> TagNames { get; set; }
        public DbSet<TagEntry> TagEntries { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}