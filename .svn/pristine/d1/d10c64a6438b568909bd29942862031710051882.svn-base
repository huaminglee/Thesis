using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Thesis.Common.Abstracts;
using Thesis.Entities;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;
using System.Web.Security;

namespace Settings
{
    public class UserManagementRepository : BaseRepository<ThesisObjectContext>, IUserManagementRepository
    {
        #region Compiled Queries

        static readonly Func<ThesisObjectContext, Guid, aspnet_Users> cqGetById = CompiledQuery.Compile<ThesisObjectContext, Guid, aspnet_Users>(
            (ctx, userId) => ctx.aspnet_Users.Where(ti => ti.UserId == userId).FirstOrDefault());

        static readonly Func<ThesisObjectContext, Guid, string> cqGetUserNameById = CompiledQuery.Compile<ThesisObjectContext, Guid, string>(
            (ctx, userId) => ctx.aspnet_Users.Where(ti => ti.UserId == userId).Select(p => p.UserName).FirstOrDefault());

        static readonly Func<ThesisObjectContext, Guid, UserManagementViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, Guid, UserManagementViewModel>(
                (ctx, userId) => ctx.aspnet_Users.Where(ti => ti.UserId == userId).Select(p => new UserManagementViewModel
                {
                     Email = p.aspnet_Membership.Email,
                     PasswordAnswer = p.aspnet_Membership.PasswordAnswer,
                     PasswordQuestion = p.aspnet_Membership.PasswordQuestion,
                     UserId = p.UserId,
                     UserName = p.UserName                     
                }).FirstOrDefault());

        #endregion

        #region IRepository<UserManagementViewModel,Guid> Members

        public UserManagementViewModel LoadViewModel(Guid id)
        {
            return cqLoadViewModel.Invoke(context, id);
        }

        public bool Save(bool isNew, Guid id, UserManagementViewModel viewModel, List<RuleViolation> validationResults)
        {
            var provider = Membership.Provider;
            bool isSuccess = false;
            if (isNew)
            {
                #region Create User

                if (string.IsNullOrEmpty(viewModel.Email) || string.IsNullOrEmpty(provider.GetUserNameByEmail(viewModel.Email)))
                {
                    MembershipCreateStatus status;
                    Guid providerUserKey = Guid.NewGuid();
                    provider.CreateUser(viewModel.UserName, viewModel.NewPassword, viewModel.Email, viewModel.PasswordQuestion, viewModel.PasswordAnswer, true, providerUserKey, out status);
                    if (status != MembershipCreateStatus.Success)
                    {
                        switch (status)
                        {
                            case MembershipCreateStatus.DuplicateEmail:
                                validationResults.Add(new RuleViolation(string.Format("{0} is used by another user", viewModel.Email), "Email"));
                                break;
                            case MembershipCreateStatus.DuplicateProviderUserKey:
                                validationResults.Add(new RuleViolation("Duplicate User Key", "UserName"));
                                break;
                            case MembershipCreateStatus.DuplicateUserName:
                                validationResults.Add(new RuleViolation(string.Format("{0} is used by another user", viewModel.UserName), "UserName"));
                                break;
                            case MembershipCreateStatus.InvalidAnswer:
                                validationResults.Add(new RuleViolation("Invalid Answer", "PasswordAnswer"));
                                break;
                            case MembershipCreateStatus.InvalidEmail:
                                validationResults.Add(new RuleViolation("Invalid Email", "Email"));
                                break;
                            case MembershipCreateStatus.InvalidPassword:
                                validationResults.Add(new RuleViolation("Invalid Password", "NewPassword"));
                                validationResults.Add(new RuleViolation("Invalid Password", "ConfirmPassword"));
                                break;
                            case MembershipCreateStatus.InvalidProviderUserKey:
                                validationResults.Add(new RuleViolation("Invalid User Key", "UserName"));
                                break;
                            case MembershipCreateStatus.InvalidQuestion:
                                validationResults.Add(new RuleViolation("Invalid Question", "PasswordQuestion"));
                                break;
                            case MembershipCreateStatus.InvalidUserName:
                                validationResults.Add(new RuleViolation("Invalid User Name", "UserName"));
                                break;
                            case MembershipCreateStatus.ProviderError:
                                validationResults.Add(new RuleViolation("Provider Error", "UserName"));
                                break;
                            case MembershipCreateStatus.Success:
                                break;
                            case MembershipCreateStatus.UserRejected:
                                validationResults.Add(new RuleViolation("User Rejected", "UserName"));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        viewModel.UserId = providerUserKey;
                        isSuccess = true;
                    }
                }
                else
                {
                    validationResults.Add(new RuleViolation(string.Format("{0} is used by another user", viewModel.Email), "Email"));
                }

                #endregion
            }
            else
            {
                #region Update User 
             
                isSuccess = true;  

                MembershipUser user = provider.GetUser(id, false);
                if (user.Email != viewModel.Email)
                {
                    if (string.IsNullOrEmpty(viewModel.Email) || string.IsNullOrEmpty(provider.GetUserNameByEmail(viewModel.Email)))
                    {
                        user.Email = viewModel.Email;
                        provider.UpdateUser(user);
                    }
                    else
                    {
                        validationResults.Add(new RuleViolation(string.Format("{0} is used by another user", viewModel.Email), "Email"));
                        isSuccess = false;
                    }
                }

                if (!string.IsNullOrEmpty(viewModel.OldPassword) && !string.IsNullOrEmpty(viewModel.NewPassword))
                {
                    bool isChange = user.ChangePassword(viewModel.OldPassword, viewModel.NewPassword);
                    if (!isChange) validationResults.Add(new RuleViolation("Invalid Password", "OldPassword"));
                    if(isSuccess) isSuccess = isChange;
                }
                if (!string.IsNullOrEmpty(viewModel.PasswordQuestionPassword))
                {
                    bool isChange = user.ChangePasswordQuestionAndAnswer(viewModel.PasswordQuestionPassword, viewModel.PasswordQuestion, viewModel.PasswordAnswer);
                    if (!isChange) validationResults.Add(new RuleViolation("Invalid Password", "PasswordQuestionPassword"));
                    if (isSuccess) isSuccess = isChange;
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.PasswordQuestion) && string.IsNullOrEmpty(viewModel.PasswordQuestion))
                    {
                        var mship = context.aspnet_Membership.Where(p => p.UserId == id).FirstOrDefault();
                        if (mship != null)
                        {
                            mship.PasswordQuestion = null;
                            mship.PasswordAnswer = null;
                            context.SaveChanges();
                        }
                    }
                }

                #endregion
            }

            #region Update Roles

            if (isSuccess && !string.IsNullOrEmpty(viewModel.RoleNames))
            {
                string[] existingRoles = Roles.GetRolesForUser(viewModel.UserName);
                string[] newRoles = viewModel.RoleNames.Split(',');
                bool isSameRoles = newRoles.Length == existingRoles.Length;
                if (isSameRoles)
                {
                    foreach (var role in existingRoles)
                    {
                        if (Array.IndexOf(newRoles, role) == -1)
                        {
                            isSameRoles = false;
                            break;
                        }
                    }
                }
                if (!isSameRoles)
                {
                    if (!isNew)
                    {
                        var cacheProvider = new Thesis.Caching.Providers.DefaultCacheProvider();
                        cacheProvider.Remove(string.Format("moduleInRoles_{0}", viewModel.UserName));
                    }

                    if(existingRoles != null && existingRoles.Length > 0)
                        Roles.RemoveUserFromRoles(viewModel.UserName, existingRoles);
                    if(newRoles != null && newRoles.Length > 0)
                        Roles.AddUserToRoles(viewModel.UserName, newRoles);
                }
            }

            #endregion

            return isSuccess;
        }

        public bool Delete(List<Guid> models)
        {
            bool isSuccess = true;
            var provider = Membership.Provider;
            bool isDelete;
            foreach (var model in models)
            {
                isDelete = provider.DeleteUser(cqGetUserNameById(context, model), true);
                if (isSuccess && !isDelete) isSuccess = false;
            }

            return isSuccess;
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            Guid[] roleIds = context.aspnet_Users.Where(p => p.UserName == userName).FirstOrDefault()
                .aspnet_Roles.Select(p => p.RoleId).ToArray();

            bool hasSuperRole = Thesis.Common.Helpers.Ax.HasSuperRoles(roleIds);

            if (hasSuperRole)
            {
                var query = context.aspnet_Users
                    .Select(p => new
                    {
                        UserId = p.UserId,
                        UserName = p.UserName,
                        Email = p.aspnet_Membership.Email,
                        LastLoginDate = p.aspnet_Membership.LastLoginDate,
                        CreateDate = p.aspnet_Membership.CreateDate
                    });

                return query.ToList(viewModel, out totalCount);
            }
            else
            {
                Guid[] childRoleIds = context.RolesInTree.Where(p => roleIds.Contains(p.ParentRoleId)).Select(p => p.RoleId).ToArray();

                var query = context.aspnet_Users.Where(p => p.UserName == userName || p.aspnet_Roles.Any(r => childRoleIds.Contains(r.RoleId)))
                    .Select(p => new
                    {
                        UserId = p.UserId,
                        UserName = p.UserName,
                        Email = p.aspnet_Membership.Email,
                        LastLoginDate = p.aspnet_Membership.LastLoginDate,
                        CreateDate = p.aspnet_Membership.CreateDate
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        #endregion

        #region IUserManagementRepository Members

        public IEnumerable GetUsersByRoleId(Guid roleId, IDataViewModel viewModel, out int totalCount)
        {
            var query = context.aspnet_Users.Where(p => p.aspnet_Roles.Any(r => r.RoleId == roleId))
                .Select(p => new
                {
                    UserId = p.UserId,
                    UserName = p.UserName,
                    Email = p.aspnet_Membership.Email,
                    LastLoginDate = p.aspnet_Membership.LastLoginDate,
                    CreateDate = p.aspnet_Membership.CreateDate
                });

            return query.ToList(viewModel, out totalCount);
        }

        #endregion
    }
}
