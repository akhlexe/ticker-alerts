﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TickerAlert.Infrastructure.Persistence;

#nullable disable

namespace TickerAlert.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TickerAlert.Domain.Entities.Alert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FinancialAssetId")
                        .HasColumnType("uuid");

                    b.Property<int>("PriceThreshold")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<decimal>("TargetPrice")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FinancialAssetId");

                    b.HasIndex("UserId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.Cedear", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FinancialAssetId")
                        .HasColumnType("uuid");

                    b.Property<string>("Ratio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FinancialAssetId");

                    b.ToTable("Cedears");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.FinancialAsset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FinancialAssets");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.PriceMeasure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FinancialAssetId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("MeasuredOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(15, 2)");

                    b.HasKey("Id");

                    b.HasIndex("FinancialAssetId");

                    b.ToTable("PriceMeasures");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.Watchlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Watchlists");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.WatchlistItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FinancialAssetId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WatchlistId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WatchlistId");

                    b.ToTable("WatchlistItems");
                });

            modelBuilder.Entity("TickerAlert.Infrastructure.Persistence.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", (string)null);
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.Alert", b =>
                {
                    b.HasOne("TickerAlert.Domain.Entities.FinancialAsset", "FinancialAsset")
                        .WithMany()
                        .HasForeignKey("FinancialAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TickerAlert.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialAsset");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.Cedear", b =>
                {
                    b.HasOne("TickerAlert.Domain.Entities.FinancialAsset", "FinancialAsset")
                        .WithMany()
                        .HasForeignKey("FinancialAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialAsset");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.PriceMeasure", b =>
                {
                    b.HasOne("TickerAlert.Domain.Entities.FinancialAsset", "FinancialAsset")
                        .WithMany()
                        .HasForeignKey("FinancialAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialAsset");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.WatchlistItem", b =>
                {
                    b.HasOne("TickerAlert.Domain.Entities.Watchlist", null)
                        .WithMany("WatchlistItems")
                        .HasForeignKey("WatchlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.Watchlist", b =>
                {
                    b.Navigation("WatchlistItems");
                });
#pragma warning restore 612, 618
        }
    }
}
