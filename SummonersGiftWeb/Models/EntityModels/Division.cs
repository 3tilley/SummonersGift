namespace SummonersGiftWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Division")]
    public partial class Division
    {
        public Division()
        {
            AggStats = new HashSet<AggStat>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte DivisionId { get; set; }

        [Required]
        [StringLength(50)]
        public string DivisionName { get; set; }

        [Required]
        [StringLength(50)]
        public string DivisionJson { get; set; }

        public virtual ICollection<AggStat> AggStats { get; set; }
    }
}
