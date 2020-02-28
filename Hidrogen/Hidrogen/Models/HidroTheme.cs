using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroTheme
    {
        public HidroTheme()
        {
            HidroSetting = new HashSet<HidroSetting>();
        }

        public byte Id { get; set; }
        public string ThemeName { get; set; }
        public string HidroFont { get; set; }
        public byte BaseFontSize { get; set; }
        public short BaseFontWeight { get; set; }
        public string BackgroundColor { get; set; }
        public string ThemePrimaryColor { get; set; }
        public string ThemeSecondaryColor { get; set; }
        public string ThemeHoveredColor { get; set; }
        public string ThemeDisabledColor { get; set; }
        public string TextPrimaryColor { get; set; }
        public string TextHighlightedColor { get; set; }
        public string TextDisabledColor { get; set; }
        public string LinkPrimaryColor { get; set; }
        public string LinkHoveredColor { get; set; }
        public string LinkDisabledColor { get; set; }
        public string ColorSuccess { get; set; }
        public string ColorWarning { get; set; }
        public string ColorDanger { get; set; }
        public string ReservedColor { get; set; }
        public string BaseShadow { get; set; }
        public string HoveredShadow { get; set; }
        public byte LineHeight { get; set; }
        public byte BaseRadius { get; set; }
        public byte BaseOpacity { get; set; }
        public string BaseMargin { get; set; }
        public string BasePadding { get; set; }
        public byte BorderWeight { get; set; }
        public string BorderColor { get; set; }

        public virtual ICollection<HidroSetting> HidroSetting { get; set; }
    }
}
