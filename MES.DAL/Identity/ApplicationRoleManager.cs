using MES.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace MES.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }
    }
}
