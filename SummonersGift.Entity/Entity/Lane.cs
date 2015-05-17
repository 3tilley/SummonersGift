namespace SummonersGift.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lane")]
    public partial class Lane
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte LaneId { get; set; }

        [Required]
        [StringLength(50)]
        public string LaneName { get; set; }

        [Required]
        [StringLength(50)]
        public string LaneJson { get; set; }
    }
}
