﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarRenting.Migrations
{
    [DbContext(typeof(CarRentingDBContext))]
    [Migration("20230203123019_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Car", b =>
                {
                    b.Property<string>("CarId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LicensePlateNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("CarId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("Renting", b =>
                {
                    b.Property<int>("RentingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("RentingId");

                    b.HasIndex("CarId");

                    b.ToTable("Rentings");
                });

            modelBuilder.Entity("Renting", b =>
                {
                    b.HasOne("Car", "Car")
                        .WithMany("Rentings")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("Car", b =>
                {
                    b.Navigation("Rentings");
                });
#pragma warning restore 612, 618
        }
    }
}
