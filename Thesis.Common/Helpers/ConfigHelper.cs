﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Thesis.Common.Helpers
{
    public partial class Ax
    {
        public static readonly bool EnableLoggingFilter = ConvertBoolean(GetAppSetting("EnableLoggingFilter"));

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static List<Guid> SuperRoles
        {
            get
            {
                var superRoles = GetAppSetting("SuperRoles");
                if (string.IsNullOrEmpty(superRoles)) return new List<Guid>();

                var ids = superRoles.Split(',');
                Guid roleId;
                List<Guid> roleIds = new List<Guid>();
                foreach (var id in ids)
                {
                    if (Guid.TryParse(id, out roleId))
                        roleIds.Add(roleId);
                }
                return roleIds;
            }
        }

        public static bool HasSuperRoles(Guid roleId)
        {
            if (roleId == Guid.Empty) return false;
            return Thesis.Common.Helpers.Ax.SuperRoles.Contains(roleId);
        }

        public static bool HasSuperRoles(List<Guid> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0) return false;

            bool hasSuperRole = false;
            foreach (var roleId in Thesis.Common.Helpers.Ax.SuperRoles)
            {
                if (roleIds.IndexOf(roleId) != -1)
                {
                    hasSuperRole = true;
                    break;
                }
            }
            return hasSuperRole;
        }

        public static bool HasSuperRoles(Guid[] roleIds)
        {
            bool hasSuperRole = false;
            foreach (var roleId in Thesis.Common.Helpers.Ax.SuperRoles)
            {
                if (Array.IndexOf(roleIds, roleId) != -1)
                {
                    hasSuperRole = true;
                    break;
                }
            }
            return hasSuperRole;
        }
    }
}
