using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Thesis.Authorization.Services
{
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipUser GetUser(string userName);

        //int MinPasswordLength { get; }
        //BoolMessage IsValidateUser(string userName, string password);
        //BoolMessageItem ValidateUser(string userName, string password, ref string roles);
        //BoolMessageItem CreateUser(string userName, string password, string email, ref MembershipCreateStatus status);
        //BoolMessageItem CreateUser(string userName, string password, string email, string roles, ref MembershipCreateStatus status);
        //bool ChangePassword(string userName, string oldPassword, string newPassword);
        //BoolMessage DeleteUser(string username);
        //BoolMessage SendPassword(string userName, string email, bool useEmail);
    }



    /// <summary>
    /// Forms Authentication Interface.
    /// </summary>
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignIn(string userName, string roles, bool createPersistentCookie);
        void SignOut();
    }
}
