﻿// <auto-generated />
using System;
using Bcc.Pledg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bcc.Pledg.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20201224120501_SortIndex")]
    partial class SortIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Bcc.Pledg.Models.CoordinatesBD.CoordinatesDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double>("Lng")
                        .HasColumnType("double precision");

                    b.Property<string>("SectorsDBId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SortIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SectorsDBId");

                    b.ToTable("PLEDGE_CoordinatesDB");
                });

            modelBuilder.Entity("Bcc.Pledg.Models.CoordinatesBD.SectorsCityDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PLEDGE_SectorsCityDB");
                });

            modelBuilder.Entity("Bcc.Pledg.Models.CoordinatesBD.SectorsDB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Sector")
                        .HasColumnType("text");

                    b.Property<int>("SectorsCityDBId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SectorsCityDBId");

                    b.ToTable("PLEDGE_SectorsDB");
                });

            modelBuilder.Entity("Bcc.Pledg.Models.LogData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("ActualAdress")
                        .HasColumnType("text");

                    b.Property<string>("ApartmentLayout")
                        .HasColumnType("text");

                    b.Property<string>("ApartmentLayoutCode")
                        .HasColumnType("text");

                    b.Property<decimal?>("Bargain")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("BeginDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("CityCodeKATO")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("DetailArea")
                        .HasColumnType("text");

                    b.Property<string>("DetailAreaCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FinQualityLevel")
                        .HasColumnType("text");

                    b.Property<string>("FinQualityLevelCode")
                        .HasColumnType("text");

                    b.Property<char>("IsArch")
                        .HasColumnType("character(1)");

                    b.Property<int?>("MaxCostPerSQM")
                        .HasColumnType("integer");

                    b.Property<int?>("MaxCostWithBargain")
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostPerSQM")
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostWithBargain")
                        .HasColumnType("integer");

                    b.Property<string>("RCName")
                        .HasColumnType("text");

                    b.Property<int?>("RCNameCode")
                        .HasColumnType("integer");

                    b.Property<string>("RelativityLocation")
                        .HasColumnType("text");

                    b.Property<string>("Sector")
                        .HasColumnType("text");

                    b.Property<string>("SectorCode")
                        .HasColumnType("text");

                    b.Property<string>("SectorDescription")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("TypeEstateByRef")
                        .HasColumnType("text");

                    b.Property<string>("TypeEstateCode")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<string>("WallMaterial")
                        .HasColumnType("text");

                    b.Property<int?>("WallMaterialCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.ToTable("PLEDGE_LogData");
                });

            modelBuilder.Entity("Bcc.Pledg.Models.PledgeReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ApartmentLayout")
                        .HasColumnType("text");

                    b.Property<string>("ApartmentLayoutCode")
                        .HasColumnType("text");

                    b.Property<decimal?>("Bargain")
                        .IsRequired()
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("BeginDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CityCodeKATO")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DetailArea")
                        .HasColumnType("text");

                    b.Property<string>("DetailAreaCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("MaxCostPerSQM")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("MaxCostWithBargain")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostPerSQM")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostWithBargain")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("RelativityLocation")
                        .HasColumnType("text");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SectorCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SectorDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TypeEstateByRef")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TypeEstateCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WallMaterial")
                        .HasColumnType("text");

                    b.Property<int?>("WallMaterialCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("City");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Sector");

                    b.HasIndex("TypeEstateByRef");

                    b.ToTable("PLEDGE_PledgeRefs");
                });

            modelBuilder.Entity("Bcc.Pledg.Models.PrimaryPledgeRef", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ActualAdress")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BeginDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("CityCodeKATO")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FinQualityLevel")
                        .HasColumnType("text");

                    b.Property<string>("FinQualityLevelCode")
                        .HasColumnType("text");

                    b.Property<int?>("MaxCostPerSQM")
                        .HasColumnType("integer");

                    b.Property<int?>("MinCostPerSQM")
                        .HasColumnType("integer");

                    b.Property<string>("RCName")
                        .HasColumnType("text");

                    b.Property<int?>("RCNameCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PLEDGE_PrimaryPledgeRefs");
                });

            modelBuilder.Entity("Bcc.Pledg.Models.CoordinatesBD.CoordinatesDB", b =>
                {
                    b.HasOne("Bcc.Pledg.Models.CoordinatesBD.SectorsDB", "SectorsDB")
                        .WithMany("CoordinatesDB")
                        .HasForeignKey("SectorsDBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Bcc.Pledg.Models.CoordinatesBD.SectorsDB", b =>
                {
                    b.HasOne("Bcc.Pledg.Models.CoordinatesBD.SectorsCityDB", "SectorsCityDB")
                        .WithMany("SectorsDB")
                        .HasForeignKey("SectorsCityDBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}