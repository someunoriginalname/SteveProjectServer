using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GameModel.Models;

public partial class GamesContext : IdentityDbContext<PublisherUser>
{
    public GamesContext()
    {
    }

    public GamesContext(DbContextOptions<GamesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.AppId);

            entity.ToTable("Game");

            entity.HasIndex(e => e.Developer, "IX_Game_Developer");

            entity.HasIndex(e => e.GameName, "IX_Game_GameName");

            entity.HasIndex(e => e.Players, "IX_Game_Players");

            entity.HasIndex(e => e.Price, "IX_Game_Price");

            entity.HasIndex(e => e.PublisherId, "IX_Game_PublisherID");

            entity.HasIndex(e => e.Revenue, "IX_Game_Revenue");

            entity.HasIndex(e => e.Year, "IX_Game_Year");

            entity.Property(e => e.AppId)
                .ValueGeneratedNever()
                .HasColumnName("AppID");
            entity.Property(e => e.Developer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GameName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Revenue).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Games)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_Publisher");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("Publisher");

            entity.HasIndex(e => e.PublisherName, "IX_Publisher_PublisherName");

            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
