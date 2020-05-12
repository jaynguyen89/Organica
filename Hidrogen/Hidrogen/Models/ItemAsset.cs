using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ItemAsset
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string AssetName { get; set; }
        public string AssetLocation { get; set; }
        public bool IsPhoto { get; set; }
        public bool IncludeInBody { get; set; }
        public bool IncludeInCover { get; set; }

        public virtual Item Item { get; set; }
    }
}
