using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Interfaces;
using System.Collections;
using Thesis.Common.ViewModels;

namespace Settings
{
    public interface IUserManagementRepository : IRepository<UserManagementViewModel, Guid>
    {
        IEnumerable GetUsersByRoleId(Guid roleId, IDataViewModel viewModel, out int totalCount);
    }
}
