using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core
{
    public class Role
    {
        public static readonly string[] AvailableRoles = { Admin, User };

        public const string Admin = "admin";
        public const string User = "user";

        public static bool IsValid(string role)
            => AvailableRoles.Contains(role);
    }
}
