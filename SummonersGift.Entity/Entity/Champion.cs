namespace SummonersGift.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Champion")]
    public partial class Champion
    {
        public string Version { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string full { get; set; }

        public string group { get; set; }

        public long? h { get; set; }

        public string sprite { get; set; }

        public long? w { get; set; }

        public long? x { get; set; }

        public long? y { get; set; }
    }
}
