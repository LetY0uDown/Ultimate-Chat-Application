using Microsoft.EntityFrameworkCore;
using Models;

namespace Host.Services;

public partial class ChatContext : DbContext
{
    public ChatContext ()
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatMember> ChatMembers { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=root;password=password;database=chat", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Chat>(entity => {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("chat");

            entity.HasIndex(e => e.CreatorId, "FK_chat_CreatorID");

            entity.Property(e => e.ID)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("ID");
            entity.Property(e => e.CreatorId)
                .HasMaxLength(36)
                .HasColumnName("CreatorID");
            entity.Property(e => e.Title)
                .HasMaxLength(36)
                .HasDefaultValueSql("''");

            entity.HasOne(d => d.Creator).WithMany(p => p.Chats)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_chat_CreatorID");
        });

        modelBuilder.Entity<ChatMember>(entity => {
            entity
                .HasNoKey()
                .ToTable("chat-member");

            entity.HasIndex(e => e.ChatId, "FK_chat-member_ChatID");

            entity.HasIndex(e => e.MemberId, "FK_chat-member_MemberID");

            entity.Property(e => e.ChatId)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("ChatID");
            entity.Property(e => e.MemberId)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("MemberID");

            entity.HasOne(d => d.Chat).WithMany()
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_chat-member_ChatID");

            entity.HasOne(d => d.Member).WithMany()
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_chat-member_MemberID");
        });

        modelBuilder.Entity<Message>(entity => {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("message");

            entity.HasIndex(e => e.ChatId, "FK_message_chatID");

            entity.HasIndex(e => e.SenderId, "FK_message_senderID");

            entity.Property(e => e.ID)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("ID");
            entity.Property(e => e.ChatId)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("chatID");
            entity.Property(e => e.SendedDate)
                .HasDefaultValueSql("'2001-01-20 00:00:00'")
                .HasColumnType("datetime");
            entity.Property(e => e.SenderId)
                .HasMaxLength(36)
                .HasColumnName("senderID");
            entity.Property(e => e.Text)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_message_chatID");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_message_senderID");
        });

        modelBuilder.Entity<User>(entity => {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.ID)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Username)
                .HasMaxLength(16)
                .HasDefaultValueSql("''");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
}
