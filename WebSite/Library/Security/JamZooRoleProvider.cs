using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.Service;

namespace Begonia.Toyota.WebSite.Library.Security
{
    public class BasicRoleProvider : DefaultRoleProvider
    {
        // Emp SN
        public override string[] GetRolesForUser(string userid)
        {
            AccountService service = new AccountService();
            AccountModel m = service.Get("role provider", userid);

            if (m != null)
            {
                return m.PermissionList.Select(p => p.ToString()).ToArray<string>();
            }

            return null;
        }
    }
}