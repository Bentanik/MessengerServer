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
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ChatHistoryEntity> ChatHistories { get; set; }
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

        #region MessageEntity
        modelBuilder.Entity<MessageEntity>()
         .HasOne(m => m.Sender)
         .WithMany(u => u.SentMessages)
         .HasForeignKey(m => m.SenderId)
         .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MessageEntity>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        #endregion
        
        #region ChatHistoryEntity
        modelBuilder.Entity<ChatHistoryEntity>()
                .HasOne(ch => ch.User)
                .WithMany()
                .HasForeignKey(ch => ch.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatHistoryEntity>()
                .HasOne(ch => ch.ChatPartner)
                .WithMany()
                .HasForeignKey(ch => ch.ChatPartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        #endregion
    }
    #endregion
}
