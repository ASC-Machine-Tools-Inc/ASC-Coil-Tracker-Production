namespace ASC_Coil_Tracker_Production.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USER")]
    public partial class USERS
    {
        public int ID { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string EMAIL { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string PASSWORD_HASH { get; set; }
    }
}