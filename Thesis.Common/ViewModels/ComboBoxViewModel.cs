﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thesis.Common.ViewModels
{
    public class ComboBoxViewModel
    {
        public int Start { get; set; }
        public int Limit { get; set; }
        public string Filter { get; set; }
        public string DisplayField { get; set; }
    }
}
