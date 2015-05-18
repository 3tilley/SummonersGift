
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SummonersGift.Models.Entity
{
    [Table("Region")]
    public partial class Region
    {
        public Region()
        {
            AggStats = new HashSet<AggStat>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte RegionId { get; set; }

        [Required]
        [StringLength(50)]
        public string RegionName { get; set; }

        [Required]
        [StringLength(50)]
        public string RegionJson { get; set; }

        public virtual ICollection<AggStat> AggStats { get; set; }
    }
}
