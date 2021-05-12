namespace ASC_Coil_Tracker_Production.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COILTABLEHISTORY")]
    public partial class COILTABLEHISTORY
    {
        public int ID { get; set; }

        [Column(TypeName = "smalldatetime")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date (MM/dd/yyyy)")]
        public DateTime DATE { get; set; }

        [Display(Name = "Amount Used")]
        public int? AMOUNTUSED { get; set; }

        [StringLength(50, ErrorMessage = "Purpose used for cannot be longer than 50 characters.")]
        [Display(Name = "What Coil Used For")]
        public string USEDFOR { get; set; }

        [StringLength(50, ErrorMessage = "Machine used in cannot be longer than 50 characters.")]
        [Display(Name = "Machine Used In")]
        public string MACHINEUSEDIN { get; set; }

        [StringLength(50, ErrorMessage = "Notes cannot be longer than 50 characters.")]
        [Display(Name = "Notes")]
        public string NOTES { get; set; }

        [Display(Name = "Coil ID")]
        public int COILID { get; set; }

        public virtual COILTABLE COILTABLE { get; set; }
    }
}
