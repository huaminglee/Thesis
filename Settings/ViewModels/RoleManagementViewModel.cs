using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Authorization.ViewModels;

namespace Settings
{
    public class RoleManagementViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get { return string.IsNullOrEmpty(RoleName) ? RoleName : RoleName.ToLower(); } }
        public string Description { get; set; }
        public List<ModulesInRolesViewModel> ModulesInRolesViewModel { get; set; }
    }
}
