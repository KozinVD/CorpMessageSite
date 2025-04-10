using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Messendger.Entities;

public partial class MessendgerDb : IdentityDbContext<User>
{
    public MessendgerDb()
    {
    }

    public MessendgerDb(DbContextOptions<MessendgerDb> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatParticipant> ChatParticipants { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<FriendRequest> FriendRequests { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserImage> UserImages { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-H8NBA30;Database=MessendgerDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsGroup).HasColumnName("isGroup");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ChatParticipant>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdChat).HasColumnName("id_chat");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdChatNavigation).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.IdChat)
                .HasConstraintName("FK_ChatParticipants_Chats");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_ChatParticipants_Users");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdMessage).HasColumnName("id_message");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.IdMessageNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.IdMessage)
                .HasConstraintName("FK_Files_Messages");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdFriend).HasColumnName("id_friend");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdFriendNavigation).WithMany(p => p.FriendIdFriendNavigations)
                .HasForeignKey(d => d.IdFriend)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friends_Users");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FriendIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friends_Users1");
        });

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateSend).HasColumnName("date_send");
            entity.Property(e => e.IdRecipient).HasColumnName("id_recipient");
            entity.Property(e => e.IdSender).HasColumnName("id_sender");

            entity.HasOne(d => d.IdRecipientNavigation).WithMany(p => p.FriendRequestIdRecipientNavigations)
                .HasForeignKey(d => d.IdRecipient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendRequests_Users");

            entity.HasOne(d => d.IdSenderNavigation).WithMany(p => p.FriendRequestIdSenderNavigations)
                .HasForeignKey(d => d.IdSender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendRequests_Users1");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdChat).HasColumnName("id_chat");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .HasColumnName("text");

            entity.HasOne(d => d.IdChatNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdChat)
                .HasConstraintName("FK_Messages_Chats");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Messages_Users");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateEnd)
                .HasColumnType("datetime")
                .HasColumnName("date_end");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Tasks_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdPhoto).HasColumnName("id_photo");
            entity.Property(e => e.LastLoginDate).HasColumnName("last_login_date");

            entity.HasOne(d => d.IdPhotoNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdPhoto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Users_UserImages");
        });

        modelBuilder.Entity<UserImage>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.ToTable("UserInfo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.IdJob).HasColumnName("id_job");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.UserInfo)
                .HasForeignKey<UserInfo>(d => d.Id)
                .HasConstraintName("FK_UserInfo_Users");

            entity.HasOne(d => d.IdJobNavigation).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.IdJob)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserInfo_Jobs");
        });
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => e.UserId);
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
