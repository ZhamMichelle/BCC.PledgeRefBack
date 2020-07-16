﻿// <auto-generated />
using System;
using BCC.PledgeRefBack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BCC.PledgeRefBack.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20200715121119_KatoCodeToString")]
    partial class KatoCodeToString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("BCC.PledgeRefBack.Models.PledgeReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ApartmentLayout")
                        .HasColumnType("text");

                    b.Property<string>("ApartmentLayoutCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BeginDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("CityCodeKATO")
                        .HasColumnType("text");

                    b.Property<decimal>("Corridor")
                        .HasColumnType("numeric");

                    b.Property<string>("DetailArea")
                        .HasColumnType("text");

                    b.Property<string>("DetailAreaCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("MaxCostPerSQM")
                        .HasColumnType("integer");

                    b.Property<int?>("MaxCostWithBargain")
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostPerSQM")
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostWithBargain")
                        .HasColumnType("integer");

                    b.Property<string>("RelativityLocation")
                        .HasColumnType("text");

                    b.Property<int?>("Sector")
                        .HasColumnType("integer");

                    b.Property<string>("SectorCode")
                        .HasColumnType("text");

                    b.Property<string>("SectorDescription")
                        .HasColumnType("text");

                    b.Property<string>("TypeEstate")
                        .HasColumnType("text");

                    b.Property<string>("TypeEstateByRef")
                        .HasColumnType("text");

                    b.Property<string>("TypeEstateCode")
                        .HasColumnType("text");

                    b.Property<string>("WallMaterial")
                        .HasColumnType("text");

                    b.Property<int?>("WallMaterialCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PledgeRefs");
                });
#pragma warning restore 612, 618
        }
    }
}
