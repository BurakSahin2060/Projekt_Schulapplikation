using Microsoft.EntityFrameworkCore;
using Schulwebapplikation.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Schulwebapplikation
{
    public class SchulDbContext : DbContext
    {
        public DbSet<Schueler> Schueler { get; set; }
        public DbSet<Klassenraum> Klassenraeume { get; set; }

        public SchulDbContext(DbContextOptions<SchulDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguriere Vererbung (Person ist abstrakt)
            modelBuilder.Entity<Schueler>().HasBaseType<Person>();
        }
    }
}
