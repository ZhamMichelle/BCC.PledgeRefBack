using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCC.PledgeRefBack.Models;

namespace BCC.PledgeRefBack
{
    public class PostgresContext : DbContext
    {
        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }
        public DbSet<PledgeReference> PledgeRefs { get; set; }
        public PostgresContext()
        {
            // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PledgeReference>().HasKey(u => u.Id);
        }
    }
}
