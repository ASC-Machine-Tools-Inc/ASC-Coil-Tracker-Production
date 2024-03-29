namespace ASC_Coil_Tracker_Production.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("COILTABLE")]
    public partial class COILTABLE
    {
        public COILTABLE()
        {
            COILTABLEHISTORY = new HashSet<COILTABLEHISTORY>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Color/Finish")]
        public string COLOR { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Material")]
        public string TYPE { get; set; }

        [RegularExpression(@"^#+([4-9]|[1-2][0-9]|30)$", ErrorMessage = "Must start with a # and be in the range 4 to 30.")]
        [StringLength(50)]
        [Required]
        [Display(Name = "Gauge")]
        public string GAUGE { get; set; }

        [Display(Name = "Thickness (in)")]
        [Range(0.001, 1.000, ErrorMessage = "Enter a thickness between 0.000 and 1.000.")]
        public double? THICK { get; set; }

        [Display(Name = "Weight (lbs)")]
        [Range(0.000, Int32.MaxValue, ErrorMessage = "Weight must be non-negative.")]
        public double? WEIGHT { get; set; }

        [Display(Name = "Length (ft)")]
        [Range(0.0, Int32.MaxValue, ErrorMessage = "Length must be non-negative.")]
        public double? LENGTH { get; set; }

        [StringLength(100, ErrorMessage = "Notes cannot be longer than 100 characters.")]
        [Display(Name = "Notes")]
        public string NOTES { get; set; }

        [Display(Name = "Yield (ksi)")]
        [Range(0.000, Int32.MaxValue, ErrorMessage = "Yield must be non-negative.")]
        public double? YIELD { get; set; }

        [Display(Name = "Width (in)")]
        [Range(0.001, 72.000, ErrorMessage = "Enter a length between 0.000 and 72.000")]
        public double? WIDTH { get; set; }

        [StringLength(50, ErrorMessage = "Customer name cannot be longer than 50 characters.")]
        [Display(Name = "Customer")]
        public string CUSTOMER { get; set; }

        public virtual ICollection<COILTABLEHISTORY> COILTABLEHISTORY { get; set; }
    }
}