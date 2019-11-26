﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AtspmApi.Models
{
    [DataContract]
    public class MovementType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int MovementTypeID { get; set; }

        [Required]
        [StringLength(30)]
        [DataMember]
        public string Description { get; set; }

        [Required]
        [StringLength(5)]
        [DataMember]
        public string Abbreviation { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }
    }
}