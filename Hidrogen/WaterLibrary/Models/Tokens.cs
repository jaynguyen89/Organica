using System;

namespace WaterLibrary.Models
{
    public partial class Tokens
    {
        public int TokenId { get; set; }
        public string TokenString { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Life { get; set; }
        public string Target { get; set; }
    }
}
