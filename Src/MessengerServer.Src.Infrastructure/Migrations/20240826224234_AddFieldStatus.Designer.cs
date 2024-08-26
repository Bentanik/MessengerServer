﻿// <auto-generated />
using System;
using MessengerServer.Src.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MessengerServer.Src.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240826224234_AddFieldStatus")]
    partial class AddFieldStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MessengerServer.Src.Domain.Entities.FriendshipEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserInitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserReceiveId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserReceiveId");

                    b.HasIndex("UserInitId", "UserReceiveId")
                        .IsUnique();

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("MessengerServer.Src.Domain.Entities.NotificationAddFriendEntitiy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FromUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("NotificationAddFriends");
                });

            modelBuilder.Entity("MessengerServer.Src.Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CropAvatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CropCoverPhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullAvatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullCoverPhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MessengerServer.Src.Domain.Entities.FriendshipEntity", b =>
                {
                    b.HasOne("MessengerServer.Src.Domain.Entities.UserEntity", "UserInitiated")
                        .WithMany("FriendshipsInitiated")
                        .HasForeignKey("UserInitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MessengerServer.Src.Domain.Entities.UserEntity", "UserReceived")
                        .WithMany("FriendshipsReceived")
                        .HasForeignKey("UserReceiveId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UserInitiated");

                    b.Navigation("UserReceived");
                });

            modelBuilder.Entity("MessengerServer.Src.Domain.Entities.UserEntity", b =>
                {
                    b.Navigation("FriendshipsInitiated");

                    b.Navigation("FriendshipsReceived");
                });
#pragma warning restore 612, 618
        }
    }
}
