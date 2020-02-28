using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroSetting
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public byte HidroThemeId { get; set; }
        public byte BirthDisplayRule { get; set; }

        public virtual HidroTheme HidroTheme { get; set; }
        public virtual Hidrogenian Hidrogenian { get; set; }
    }
}
