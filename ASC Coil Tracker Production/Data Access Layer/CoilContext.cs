using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using ASC_Coil_Tracker_Production.Models;

namespace ASC_Coil_Tracker_Production.Data_Access_Layer
{
    public partial class CoilContext : DbContext
    {
#pragma warning disable IDE0051 // Remove unused private members
        private const string TEST = "TestDatabase";
        private const string PROD = "name=db_a6e98a_kitsuragi";
#pragma warning restore IDE0051 // Remove unused private members

        public CoilContext()
            : base(PROD)
        {
        }

        public virtual DbSet<COILTABLE> Coils { get; set; }

        public virtual DbSet<COILTABLEHISTORY> History { get; set; }

        public virtual DbSet<USERS> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<COILTABLE>()
                .HasMany(e => e.COILTABLEHISTORY)
                .WithRequired(e => e.COILTABLE)
                .HasForeignKey(e => e.COILID)
                .WillCascadeOnDelete(true);

            Database.SetInitializer<CoilContext>(null);
        }
    }
}