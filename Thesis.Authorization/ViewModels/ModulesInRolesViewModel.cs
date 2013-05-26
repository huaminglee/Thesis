using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Authorization.Enumerations;

namespace Thesis.Authorization.ViewModels
{
    public class ModulesInRolesViewModel
    {
        public int ModuleId { get; set; }
        public Guid RoleId { get; set; } 
        public ProcessTypes ProcessType { get; set; }
    }
}
