using System.Collections.Generic;

namespace MES.DAL.Entities
{
    public class User : IdProvider
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual Role Role { get; set; }
        public int RoleId { get; set; }

        public virtual ICollection<Assembly> Assemblys { get; set; } = new HashSet<Assembly>();

    }
}