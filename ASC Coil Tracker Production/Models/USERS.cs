namespace ASC_Coil_Tracker_Production.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USERS")]
    public partial class USERS
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string PasswordHash { get; set; }
    }
}