namespace SummonersGiftWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tier")]
    public partial class Tier
    {
        public Tier()
        {
            AggStats = new HashSet<AggStat>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TierId { get; set; }

        [Required]
        [StringLength(50)]
        public string TierName { get; set; }

        [Required]
        [StringLength(50)]
        public string TierJson { get; set; }

        public virtual ICollection<AggStat> AggStats { get; set; }
    }
}
