namespace SummonersGift.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Champion")]
    public partial class Champion
    {
        public Champion()
        {
            AggStats = new HashSet<AggStat>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ChampionId { get; set; }

        [Required]
        [StringLength(50)]
        public string Version { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<AggStat> AggStats { get; set; }
    }
}
