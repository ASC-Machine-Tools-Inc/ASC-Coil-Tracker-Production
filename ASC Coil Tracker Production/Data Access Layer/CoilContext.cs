using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using ASC_Coil_Tracker_Production.Models;

namespace ASC_Coil_Tracker_Production.Data_Access_Layer
{
    public partial class CoilContext : DbContext
    {
        public CoilContext()
            : base("name=JOBSHEETSPOCK")
        {
        }

        public virtual DbSet<COILTABLE> Coils { get; set; }

        public virtual DbSet<COILTABLEHISTORY> History { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<COILTABLE>()
                .HasMany(e => e.COILTABLEHISTORY)
                .WithRequired(e => e.COILTABLE)
                .HasForeignKey(e => e.COILID)
                .WillCascadeOnDelete(true);
        }
    }
}
