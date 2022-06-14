﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rental_Property_Management_Tool.Data;

namespace Rental_Property_Management_Tool.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220614114056_OverheadCost")]
    partial class OverheadCost
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.OverheadCost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RentalPropertyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RentalPropertyId");

                    b.ToTable("OverheadCosts");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Contact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LegalEntity")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.RentalProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersonsId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RentalEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RentalStart")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Rented")
                        .HasColumnType("bit");

                    b.Property<double>("SquaresMeters")
                        .HasColumnType("float");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("PersonsId");

                    b.HasIndex("UserId");

                    b.ToTable("RentalProperties");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.OverheadCost", b =>
                {
                    b.HasOne("Rental_Property_Management_Tool.Entities.RentalProperty", "RentalProperty")
                        .WithMany("Costs")
                        .HasForeignKey("RentalPropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RentalProperty");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.RentalProperty", b =>
                {
                    b.HasOne("Rental_Property_Management_Tool.Entities.Person", "Persons")
                        .WithMany("RentedProperties")
                        .HasForeignKey("PersonsId");

                    b.HasOne("Rental_Property_Management_Tool.Entities.User", "User")
                        .WithMany("RentalProperties")
                        .HasForeignKey("UserId");

                    b.Navigation("Persons");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.Person", b =>
                {
                    b.Navigation("RentedProperties");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.RentalProperty", b =>
                {
                    b.Navigation("Costs");
                });

            modelBuilder.Entity("Rental_Property_Management_Tool.Entities.User", b =>
                {
                    b.Navigation("RentalProperties");
                });
#pragma warning restore 612, 618
        }
    }
}
