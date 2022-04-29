﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VacationRental.DAL.Core;

#nullable disable

namespace VacationRental.DAL.Core.Migrations
{
    [DbContext(typeof(VacationRentalContext))]
    [Migration("20220426115010_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("VacationRental.Entities.Core.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<int>("Nights")
                        .HasColumnType("int");

                    b.Property<int>("RentalId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RentalId");

                    b.ToTable("Booking", "vc");
                });

            modelBuilder.Entity("VacationRental.Entities.Core.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("PreparationTime")
                        .HasColumnType("int");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rental", "vc");
                });

            modelBuilder.Entity("VacationRental.Entities.Core.Booking", b =>
                {
                    b.HasOne("VacationRental.Entities.Core.Rental", "Rental")
                        .WithMany("Bookings")
                        .HasForeignKey("RentalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rental");
                });

            modelBuilder.Entity("VacationRental.Entities.Core.Rental", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
