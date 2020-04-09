using System.Collections.Generic;

namespace WaterLibrary.Models
{
    public class Photos
    {
        public Photos()
        {
            Userphotos = new HashSet<Userphotos>();
        }

        public int Id { get; set; }
        public string PhotoName { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Userphotos> Userphotos { get; set; }
    }
}
