using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<CastEntry> CastEntries { get; set; }
        public DbSet<CastType> CastTypes { get; set; }
        public DbSet<FavoriteEntry> FavoriteEntries { get; set; }
        public DbSet<TagEntry> TagEntries { get; set; }
        public DbSet<User> Users { get; set; }
    }
}