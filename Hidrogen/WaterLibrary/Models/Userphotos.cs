namespace WaterLibrary.Models
{
    public partial class Userphotos
    {
        public int Id { get; set; }
        public int? PhotoId { get; set; }
        public int? HidrogenianId { get; set; }
        public bool? IsAvatar { get; set; }
        public bool? IsCover { get; set; }

        public virtual Photos Photo { get; set; }
    }
}
