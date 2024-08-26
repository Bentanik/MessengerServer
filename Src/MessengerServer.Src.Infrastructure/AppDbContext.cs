using MessengerServer.Src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerServer.Src.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    #region DbSet
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<FriendshipEntity> Friendships { get; set; }
    public DbSet<NotificationAddFriendEntitiy> NotificationAddFriends { get; set; }
    #endregion

    #region OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FriendshipEntity>()
            .HasOne(f => f.UserInitiated)
            .WithMany(u => u.FriendshipsInitiated)
            .HasForeignKey(f => f.UserInitId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FriendshipEntity>()
            .HasOne(f => f.UserReceived)
            .WithMany(u => u.FriendshipsReceived)
            .HasForeignKey(f => f.UserReceiveId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FriendshipEntity>()
            .HasIndex(f => new { f.UserInitId, f.UserReceiveId })
            .IsUnique();
    }
    #endregion
}
