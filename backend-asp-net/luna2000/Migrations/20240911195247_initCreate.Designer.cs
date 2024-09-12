﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using luna2000.Data;

#nullable disable

namespace luna2000.Migrations
{
    [DbContext(typeof(LunaDbContext))]
    [Migration("20240911195247_initCreate")]
    partial class initCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.33");

            modelBuilder.Entity("luna2000.Models.DriverEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Contacts")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DriverLicense")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Fio")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ParkingAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Passport")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Registration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("luna2000.Models.PhotoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DriverId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DriverId1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DriverId1");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("luna2000.Models.PhotoEntity", b =>
                {
                    b.HasOne("luna2000.Models.DriverEntity", "Driver")
                        .WithMany("Photos")
                        .HasForeignKey("DriverId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("luna2000.Models.DriverEntity", b =>
                {
                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
