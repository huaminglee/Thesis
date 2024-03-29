﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Activity;
using Thesis.Authorization.Services;
using Crm;
using Ninject;
using Ninject.Modules;
using Settings;
using Shell;

namespace Thesis.Infrastructure
{
    // Configures how abstract service types are mapped to concrete implementations
    public class AxiomModuleServices : NinjectModule
    {
        public override void Load()
        {
            #region Shell

            Bind<IFilesRepository>().To<FilesRepository>();

            #endregion

            #region Authorization

            Bind<IFormsAuthenticationService>().To<FormsAuthenticationService>();
            Bind<IMembershipService>().To<AccountMembershipService>();

            #endregion

            #region Crm

            Bind<IAddressRepository>().To<AddressRepository>();
            Bind<IRoofRepository>().To<RoofRepository>();
            Bind<IRelationInAddressRepository>().To<RelationInAddressRepository>();

            #endregion

            #region Activity

            Bind<IActivityRepository>().To<ActivityRepository>();

            #endregion

            #region Settings

            Bind<ILookupRepository>().To<LookupRepository>();
            Bind<IUserManagementRepository>().To<UserManagementRepository>();
            Bind<IRoleManagementRepository>().To<RoleManagementRepository>();
            Bind<IBaseTableRepository>().To<BaseTableRepository>();

            #endregion
        }
    }
}