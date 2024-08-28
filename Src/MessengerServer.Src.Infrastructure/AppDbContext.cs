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
        #region FriendshipEntity
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
        #endregion

        #region NotificationAddFriendEntitiy
        modelBuilder.Entity<NotificationAddFriendEntitiy>()
            .HasOne(n => n.FromUser)
            .WithMany(u => u.SentNotificationAddFriends)
            .HasForeignKey(n => n.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NotificationAddFriendEntitiy>()
            .HasOne(n => n.ToUser)
            .WithMany(u => u.ReceivedNotificationAddFriends)
            .HasForeignKey(n => n.ToUserId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
    #endregion
}
