using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using GameModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameModel;

public partial class GameContext : DbContext
{
    public GameContext()
    {
    }

    public GameContext(DbContextOptions<GameContext> options)
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
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.AppID).HasName("PK_Game");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Games)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_Publisher");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherID).HasName("PK_Publisher");

            entity.Property(e => e.PublisherID).ValueGeneratedNever();
            entity.Property(e => e.PublisherName).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
