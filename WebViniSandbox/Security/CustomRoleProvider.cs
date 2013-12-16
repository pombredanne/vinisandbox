using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ViniSandbox.Models;

namespace WebViniSandbox.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        private vinisandboxContext db = new vinisandboxContext();

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return new string[]{ "admin" };
        }

        public override string[] GetRolesForUser(string username)
        {
            if (db.users.Count(p => p.email == username && p.admin) > 0)
                return new string[] { "admin" };
            return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (roleName == "admin")
            {
                return db.users.Where(p => p.admin).Select<user, string>(p => p.email).ToArray();
            }
            return new string[] { };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName == "admin")
                return db.users.Count(p => p.name == username && p.admin) > 0;
            return false;

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return roleName == "admin";
        }
    }
}