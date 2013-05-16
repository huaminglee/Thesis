using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Thesis.Common.Helpers;

namespace Thesis.Authorization.Services
{
    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            Ax.ValidateRequiredStringValue(userName, "userName");
            Ax.ValidateRequiredStringValue(password, "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            Ax.ValidateRequiredStringValue(userName, "userName");
            Ax.ValidateRequiredStringValue(password, "password");
            Ax.ValidateRequiredStringValue(email, "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            Ax.ValidateRequiredStringValue(userName, "userName");
            Ax.ValidateRequiredStringValue(oldPassword, "oldPassword");
            Ax.ValidateRequiredStringValue(newPassword, "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public MembershipUser GetUser(string userName)
        {
            return _provider.GetUser(userName, true /* userIsOnline */);
        }

        public MembershipUser GetUser(string userName, bool userIsOnline)
        {
            return _provider.GetUser(userName, userIsOnline);
        }

        public string GetUserNameByEmail(string email)
        {
            return _provider.GetUserNameByEmail(email);
        }
    }
}
