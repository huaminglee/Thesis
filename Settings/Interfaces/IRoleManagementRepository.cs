using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Entities;
using Thesis.Common.ViewModels;
using System.Collections;
using Thesis.Common.Interfaces;

namespace Settings
{
    public interface IRoleManagementRepository : IRepository<RoleManagementViewModel, Guid>
    {
        IEnumerable GetRoles(ComboBoxViewModel viewModel, out int totalCount);
        IEnumerable GetRolesByUserId(Guid userId, IDataViewModel viewModel, out int totalCount);
    }
}
