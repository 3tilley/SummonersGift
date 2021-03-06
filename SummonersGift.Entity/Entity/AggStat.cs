namespace SummonersGift.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AggStat
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int index { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte TierId { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte DivisionId { get; set; }

        [Key]
        [Column(Order = 3)]
        public byte RoleId { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ChampionId { get; set; }

        public bool? IsBlue { get; set; }

        public bool? Winner { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short StatId { get; set; }

        public double? mean { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int count { get; set; }

        public double? std { get; set; }

        public double? sem { get; set; }

        [Key]
        [Column(Order = 7)]
        public byte MapId { get; set; }

        [Key]
        [Column(Order = 8)]
        public byte RegionId { get; set; }

        [Key]
        [Column(Order = 9)]
        public byte DataVersion { get; set; }

        public byte? Minute { get; set; }
    }
}
