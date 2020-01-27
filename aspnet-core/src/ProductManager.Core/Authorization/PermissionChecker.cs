using Abp.Authorization;
using ProductManager.Authorization.Roles;
using ProductManager.Authorization.Users;

namespace ProductManager.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
