﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thesis.Common.Enumerations
{
    public enum FilterCondition
    {
        StartsWith = 1,
        EndsWith = 2,
        Contains = 3,
        Equals = 4,
        NotEqual = 5,
        Greater = 6,
        Less = 7,
        Between = 8
    }
    
    public enum PredicateCondition
    {
        And = 1,
        Or = 2
    }
}
