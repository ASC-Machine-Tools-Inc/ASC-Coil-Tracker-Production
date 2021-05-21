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

        [Display(Name = "Amount Used (ft)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Amount used must be non-negative.")]
        public int? AMOUNTUSED { get; set; }

        [StringLength(50, ErrorMessage = "Job numbers cannot be longer than 50 characters.")]
        [Display(Name = "Assigned Job Number")]
        public string JOBNUMBER { get; set; }

        [StringLength(50, ErrorMessage = "Notes cannot be longer than 50 characters.")]
        [Display(Name = "Notes")]
        public string NOTES { get; set; }

        [Display(Name = "Coil ID")]
        public int COILID { get; set; }

        public virtual COILTABLE COILTABLE { get; set; }
    }
}