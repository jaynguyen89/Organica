using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ItemDetail
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Condition { get; set; }
        public string Quality { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Storage { get; set; }
        public int? Memory { get; set; }
        public string ProductionNote { get; set; }
        public string Color { get; set; }
        public string CarrierName { get; set; }
        public string LockStatus { get; set; }
        public string MadeIn { get; set; }
        public short? Warranty { get; set; }
        public string WarrantedBy { get; set; }
        public string Size { get; set; }
        public string Dimensions { get; set; }
        public string FormFactor { get; set; }
        public string Certification { get; set; }
        public string Materials { get; set; }
        public short? Measurement { get; set; }
        public string Packaging { get; set; }
        public string Version { get; set; }
        public string Processing { get; set; }
        public string SerialOrSku { get; set; }
        public string ToBeUsedFor { get; set; }
        public string CompatibleWith { get; set; }

        public virtual Item Item { get; set; }
    }
}
