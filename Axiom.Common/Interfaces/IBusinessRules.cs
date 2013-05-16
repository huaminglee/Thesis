using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Models;

namespace Thesis.Common.Interfaces
{
    public interface IBusinessRules
    {
        void Validate(List<RuleViolation> validationResults);
    }
}
