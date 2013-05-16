using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Thesis.Common.Attributes;
using Thesis.Common.Interfaces;
using Thesis.Common.Models;

namespace Thesis.Entities
{
    [MetadataType(typeof(Thesis.Entities.MetadataTypes.AddressesMD))]
    public partial class Addresses: IBusinessRules
    {

        #region IBusinessRules Members

        public void Validate(List<RuleViolation> errors)
        {
            if (!string.IsNullOrEmpty(this.Street) && this.Street.Length > 5)
            {
                errors.Add(new RuleViolation("Dogru duzgun deger gir!", "Street"));
            }
        }

        #endregion
    }
}
