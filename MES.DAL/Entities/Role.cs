using System.Collections.Generic;

namespace MES.DAL.Entities
{
    public class Role : IdProvider
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}