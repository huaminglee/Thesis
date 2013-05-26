using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity;
using Thesis.Common.Helpers;
using Thesis.Common.ViewModels;
using Thesis.Controllers;
using Thesis.Lib.ActionResults;
using Thesis.Lib.Attributes;
using Thesis.Lib.Controllers;
using Thesis.Lib.Extensions;

namespace Thesis.Areas.Activity.Controllers
{
    [ModuleAuthorize]
    public class ActivityController : BaseScenarios<ActivityViewModel, IActivityRepository>
    {
        public ActivityController(IActivityRepository repository) : base(repository, "Name", "ActivityID")
        {
     
        }
    }
}
