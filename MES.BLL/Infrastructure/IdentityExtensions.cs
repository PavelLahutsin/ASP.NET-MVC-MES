using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace MES.BLL.Infrastructure
{
    public static class IdentityExtensions
    {
        public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (!(identity is ClaimsIdentity ci)) return default(T);
            var id = ci.FindFirst(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                return (T)Convert.ChangeType(id.Value, typeof(T), CultureInfo.InvariantCulture);
            }
            return default(T);
        }
        public static string GetUserRole(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            var ci = identity as ClaimsIdentity;
            var role = "";
            if (ci == null) return role;
            var id = ci.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
            if (id != null)
                role = id.Value;
            return role;
        }
    }
}