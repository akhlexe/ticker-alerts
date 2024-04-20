﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TickerAlert.Domain.Entities.Alert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FinancialAssetId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<decimal>("TargetPrice")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<int>("ThresholdType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FinancialAssetId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.FinancialAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FinancialAssets");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.PriceMeasure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FinancialAssetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MeasuredOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(15, 2)");

                    b.HasKey("Id");

                    b.HasIndex("FinancialAssetId");

                    b.ToTable("PriceMeasures");
                });

            modelBuilder.Entity("TickerAlert.Domain.Entities.Alert", b =>
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
#pragma warning restore 612, 618
        }
    }
}
