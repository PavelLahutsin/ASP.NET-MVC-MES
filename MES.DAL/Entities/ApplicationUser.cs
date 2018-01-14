using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MES.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
        //public virtual ICollection<ArrivalOfDetail> ArrivalOfDetailses { get; set; } = new HashSet<ArrivalOfDetail>();
    }
}
