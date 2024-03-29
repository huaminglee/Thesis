﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Interfaces;
using Thesis.Common.Models;

namespace Settings
{
    public class UserManagementViewModel : IBusinessRules
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        
        public string PasswordQuestionPassword { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }

        public string RoleNames { get; set; }

        #region IBusinessRules Members

        public void Validate(List<RuleViolation> validationResults)
        {
            bool isChangePassword = !string.IsNullOrEmpty(this.OldPassword) || !string.IsNullOrEmpty(this.NewPassword) || !string.IsNullOrEmpty(this.ConfirmPassword);
            if (isChangePassword)
            {
                bool hasPasswordError = false;
                if (string.IsNullOrEmpty(this.OldPassword) && this.UserId != Guid.Empty) { validationResults.Add(new RuleViolation("This field is required", "OldPassword")); hasPasswordError = true; }
                if (string.IsNullOrEmpty(this.NewPassword)) { validationResults.Add(new RuleViolation("This field is required", "NewPassword")); hasPasswordError = true; }
                if (string.IsNullOrEmpty(this.ConfirmPassword)) { validationResults.Add(new RuleViolation("This field is required", "ConfirmPassword")); hasPasswordError = true; }

                if (!hasPasswordError && this.NewPassword != this.ConfirmPassword)
                {
                    validationResults.Add(new RuleViolation("Password and Confirm Password don't match", "ConfirmPassword"));
                }
            }

            bool isChangePasswordQuestion = !string.IsNullOrEmpty(this.PasswordQuestionPassword) || !string.IsNullOrEmpty(this.PasswordAnswer);
            if (isChangePasswordQuestion)
            {
                if (string.IsNullOrEmpty(this.PasswordQuestionPassword) && this.UserId != Guid.Empty) validationResults.Add(new RuleViolation("This field is required", "PasswordQuestionPassword"));
                if (string.IsNullOrEmpty(this.PasswordQuestion)) validationResults.Add(new RuleViolation("This field is required", "PasswordQuestion"));
                if (string.IsNullOrEmpty(this.PasswordAnswer)) validationResults.Add(new RuleViolation("This field is required", "PasswordAnswer"));
            }
        }

        #endregion
    }
}
