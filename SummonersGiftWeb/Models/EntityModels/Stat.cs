namespace SummonersGiftWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stat")]
    public partial class Stat
    {
        public Stat()
        {
            AggStats = new HashSet<AggStat>();
        }

        public short StatId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatName { get; set; }

        [Required]
        [StringLength(50)]
        public string StatCode { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string JsonEndPoint { get; set; }

        [StringLength(100)]
        public string JsonPath { get; set; }

        public bool IsStraightFromJson { get; set; }

        public virtual ICollection<AggStat> AggStats { get; set; }
    }
}
