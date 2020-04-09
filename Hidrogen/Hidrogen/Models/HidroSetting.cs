namespace Hidrogen.Models
{
    public class HidroSetting
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public byte HidroThemeId { get; set; }
        public byte BirthDisplayRule { get; set; }
        public byte TwoFaMethod { get; set; }

        public virtual HidroTheme HidroTheme { get; set; }
        public virtual Hidrogenian Hidrogenian { get; set; }
    }
}
