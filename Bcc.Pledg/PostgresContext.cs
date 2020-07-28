﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bcc.Pledg.Models;

namespace Bcc.Pledg
{
    public class PostgresContext : DbContext
    {
        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }
        public DbSet<PledgeReference> PledgeRefs { get; set; }
        public DbSet<LogData> LogData { get; set; }
        public PostgresContext()
        {
            // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PledgeReference>().HasKey(u => u.Id);
            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.City).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.CityCodeKATO).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.Sector).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.SectorCode).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.SectorDescription).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.TypeEstateByRef).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.TypeEstateCode).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.MaxCostPerSQM).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.MinCostPerSQM).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.MaxCostWithBargain).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.MinCostWithBargain).IsRequired();

            modelBuilder.Entity<PledgeReference>()
                .Property(bc => bc.Bargain).IsRequired();


            modelBuilder.Entity<LogData>().HasKey(u => u.Id);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName($"PLEDGE_{entity.GetTableName()}");
            }
        }
    }
}
