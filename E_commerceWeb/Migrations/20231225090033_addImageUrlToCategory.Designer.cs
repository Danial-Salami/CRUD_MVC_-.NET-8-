﻿// <auto-generated />
using E_commerceWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_commerceWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231225090033_addImageUrlToCategory")]
    partial class addImageUrlToCategory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("E_commerceWeb.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            ImageUrl = "",
                            Name = "phone"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            ImageUrl = "",
                            Name = "computer"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            ImageUrl = "",
                            Name = "tablet"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}