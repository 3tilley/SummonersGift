namespace SummonersGift.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Summoner
    {
        [Key]
        [Column(Order = 0)]
        public string Version { get; set; }

        public string Description { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Name { get; set; }

        public byte? Summonerlevel { get; set; }

        public string Full { get; set; }

        public string Group { get; set; }

        public int? H { get; set; }

        public string Sprite { get; set; }

        public int? W { get; set; }

        public int? X { get; set; }

        public int? Y { get; set; }
    }
}
