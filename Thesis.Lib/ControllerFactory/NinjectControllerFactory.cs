﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ninject;
using System.Web.Routing;
using Ninject.Modules;

namespace Thesis.Lib.ControllerFactory
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        // A Ninject "kernel" is the thing that can supply object instances
        IKernel kernel;

        public NinjectControllerFactory(params INinjectModule[] modules)
        {
            kernel = new StandardKernel(modules);
        }

        public NinjectControllerFactory(INinjectSettings settings, params INinjectModule[] modules)
        {
            kernel = new StandardKernel(settings, modules);
        }

        // ASP.NET MVC calls this to get the controller for each request
        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            if (controllerType == null)
                return base.GetControllerInstance(context, controllerType);

            var controller = kernel.Get(controllerType) as IController;

            return controller ?? base.GetControllerInstance(context, controllerType);
        }
    }
}
