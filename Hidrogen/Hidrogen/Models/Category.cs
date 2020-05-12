using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Category
    {
        public Category()
        {
            Classification = new HashSet<Classification>();
            InverseDependant = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? DependantId { get; set; }
        public string AvatarName { get; set; }
        public string Restrictions { get; set; }

        public virtual Category Dependant { get; set; }
        public virtual ICollection<Classification> Classification { get; set; }
        public virtual ICollection<Category> InverseDependant { get; set; }
    }
}
