﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thesis.Common.Helpers
{
    public partial class Ax
    {
        public static string GetExceptionDetail(Exception ex)
        {
            StringBuilder error = new StringBuilder();
            error.AppendLine("Message : " + (ex.Message ?? string.Empty));
            error.AppendLine("Stack Trace : " + (ex.StackTrace ?? string.Empty));
            error.AppendLine("Inner Exception : " + (ex.InnerException.ToString() ?? string.Empty));

            return error.ToString();
        }
    }
}
