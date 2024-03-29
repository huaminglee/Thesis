﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Thesis.Common.Helpers;
using Thesis.Authorization.Helpers;

namespace Thesis.Authorization.Services
{
    /// <summary>
    /// Forms Authentication Implementation
    /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        /// <summary>
        /// Sign in issuing the Forms Authentication Ticket.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(string userName, bool createPersistentCookie)
        {
            SignIn(userName, string.Empty, createPersistentCookie);
        }


        /// <summary>
        /// Sign in issuing the Forms Authentication Ticket.
        /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(string userName, string roles, bool createPersistentCookie)
        {
            Ax.ValidateRequiredStringValue(userName, "userName");

            SecurityHelper.SetCookieContainingUserData(userName, roles, 30);
        }


        /// <summary>
        /// Sign out.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
