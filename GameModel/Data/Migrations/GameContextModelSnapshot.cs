﻿// <auto-generated />
using GameModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameModel.Data.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GameModel.Game", b =>
                {
                    b.Property<int>("AppID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AppID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppID"));

                    b.Property<string>("Developer")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Players")
                        .HasColumnType("int")
                        .HasColumnName("Players");

                    b.Property<int>("PublisherID")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("Year");

                    b.HasKey("AppID")
                        .HasName("PK_Game");

                    b.HasIndex("PublisherID");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("GameModel.Publisher", b =>
                {
                    b.Property<int>("PublisherID")
                        .HasColumnType("int")
                        .HasColumnName("PublisherID");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("char(100)")
                        .IsFixedLength();

                    b.HasKey("PublisherID")
                        .HasName("PK_Publisher");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("GameModel.Game", b =>
                {
                    b.HasOne("GameModel.Publisher", "Publisher")
                        .WithMany("Games")
                        .HasForeignKey("PublisherID")
                        .IsRequired()
                        .HasConstraintName("FK_Game_Publisher");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("GameModel.Publisher", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}