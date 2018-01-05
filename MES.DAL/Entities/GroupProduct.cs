using System.Collections.Generic;

namespace MES.DAL.Entities
{
    public class GroupProduct : IdProvider
    {
        public string Name { get; set; }

        public virtual ICollection<Detail> Details { get; set; } = new HashSet<Detail>();

    }
}
