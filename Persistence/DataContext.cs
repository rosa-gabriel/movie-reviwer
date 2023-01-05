using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Views;

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
        public DbSet<CastRole> CastRoles { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<ConfirmationEmailToken> ConfirmationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasMany<Friend>(u => u.SentRequests).WithOne(fr => fr.Sender);
            builder.Entity<User>().HasMany<Friend>(u => u.ReceivedRequests).WithOne(fr => fr.Receiver);
        }
    }
}