using Microsoft.AspNet.Identity.EntityFramework;

namespace MES.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
