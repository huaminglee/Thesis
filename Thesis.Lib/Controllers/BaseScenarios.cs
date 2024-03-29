﻿using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Thesis.Common.Helpers;
using Thesis.Common.Interfaces;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;

using Thesis.Lib.ActionResults;
using Thesis.Lib.Attributes;
using Thesis.Lib.Extensions;

namespace Thesis.Lib.Controllers
{
    #region Generic Id Scenarios

    public class BaseScenarios<TViewModel, TRepository, TIdType> : ThesisBaseController<TRepository>
        where TRepository : IRepository<TViewModel, TIdType>
        where TViewModel : class, new()
        where TIdType : struct
    {
        string tabTitle, tabId;
        TIdType emptyValue;
        bool getByValueProvider, isUpload;

        public BaseScenarios(TRepository repository, string tabTitle, string tabId, TIdType emptyValue) : this(repository, tabTitle, tabId, emptyValue, false, false)
        {
   
        }

        public BaseScenarios(TRepository repository, string tabTitle, string tabId, TIdType emptyValue, bool getByValueProvider) : this(repository, tabTitle, tabId, emptyValue, getByValueProvider, false)
        {
            
        }

        public BaseScenarios(TRepository repository, string tabTitle, string tabId, TIdType emptyValue, bool getByValueProvider, bool isUpload) : base(repository)
        {
            this.tabTitle = tabTitle;
            this.tabId = tabId;
            this.emptyValue = emptyValue;
            this.getByValueProvider = getByValueProvider;
            this.isUpload = isUpload;
        }

        [View(null)]
        [HttpGet]
        [CompressFilter]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public virtual ActionResult List()
        {
            return View();
        }

        [View(null)]
        [HttpPost]
        public virtual ActionResult GetByFilter(GridPanelViewModel model)
        {
            int totalCount;
            var data = repository.GetByFilter(model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }

        [Delete]
        [HttpPost]
        public virtual ActionResult Delete(List<TIdType> rows)
        {
            return new DeleteResult(repository.Delete(rows), rows.Count);
        }

        //[View(emptyValue)]
        [HttpGet]
        [CompressFilter]
        public virtual ActionResult Detail(TIdType? id)
        {
            if (!id.HasValue) id = emptyValue;
            var model = id.Equals(emptyValue) ?
                        new TViewModel() :
                        repository.LoadViewModel(id.Value);

            return View(model ?? new TViewModel());
        }

        //[Save(emptyValue)]
        [HttpPost]
        public virtual ActionResult Save(TIdType? id)
        {
            if (!id.HasValue) id = emptyValue;
            var viewModel = new TViewModel();

            TryUpdateModel(viewModel, ControllerContext, ModelState);

            var validationResults = new List<RuleViolation>();
            bool isNew = id.Equals(emptyValue);
            bool isValid = repository.Save(isNew, id.Value, viewModel, validationResults);
            if (isValid && validationResults.Count == 0)
            {
                if (getByValueProvider)
                    return new SaveResult(isNew, ValueProvider.GetValue(tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
                else
                    return new SaveResult(isNew, Ax.GetValue(viewModel, tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
            }
            else
                ModelState.AddModelErrors(validationResults);

            return new ErrorResult(ModelState, isUpload);
        }
    }

    public class BaseListScenarios<TRepository, TIdType> : ThesisBaseController<TRepository>
        where TRepository : IListRepository<TIdType>
        where TIdType : struct
    {
        public BaseListScenarios(TRepository repository)
            : base(repository)
        { }

        [View(null)]
        [HttpGet]
        [CompressFilter]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public virtual ActionResult List()
        {
            return View();
        }

        [View(null)]
        [HttpPost]
        public virtual ActionResult GetByFilter(GridPanelViewModel model)
        {
            int totalCount;
            var data = repository.GetByFilter(model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }

        [Delete]
        [HttpPost]
        public virtual ActionResult Delete(List<TIdType> rows)
        {
            return new DeleteResult(repository.Delete(rows), rows.Count);
        }
    }

    public class BaseDetailScenarios<TViewModel, TRepository, TIdType> : ThesisBaseController<TRepository>
        where TRepository : IDetailRepository<TViewModel, TIdType>
        where TViewModel : class, new()
        where TIdType : struct
    {
        string tabTitle, tabId;
        TIdType emptyValue;
        bool getByValueProvider, isUpload;

        public BaseDetailScenarios(TRepository repository, string tabTitle, string tabId, TIdType emptyValue) : this(repository, tabTitle, tabId, emptyValue, false, false)
        {
        }

        public BaseDetailScenarios(TRepository repository, string tabTitle, string tabId, TIdType emptyValue, bool getByValueProvider) : this(repository, tabTitle, tabId, emptyValue, getByValueProvider, false)
        {
        }

        public BaseDetailScenarios(TRepository repository, string tabTitle, string tabId, TIdType emptyValue, bool getByValueProvider, bool isUpload) : base(repository)
        {
            this.tabTitle = tabTitle;
            this.tabId = tabId;
            this.emptyValue = emptyValue;
            this.getByValueProvider = getByValueProvider;
            this.isUpload = isUpload;
        }

        [Delete]
        [HttpPost]
        public virtual ActionResult Delete(List<TIdType> rows)
        {
            return new DeleteResult(repository.Delete(rows), rows.Count);
        }

        //[View(emptyValue)]
        [HttpGet]
        [CompressFilter]
        public virtual ActionResult Detail(TIdType? id)
        {
            if (!id.HasValue) id = emptyValue;
            var model = id.Equals(emptyValue) ?
                        new TViewModel() :
                        repository.LoadViewModel(id.Value);

            return View(model ?? new TViewModel());
        }

        //[Save(emptyValue)]
        [HttpPost]
        public virtual ActionResult Save(TIdType? id)
        {
            if (!id.HasValue) id = emptyValue;
            var viewModel = new TViewModel();

            TryUpdateModel(viewModel, ControllerContext, ModelState);

            var validationResults = new List<RuleViolation>();
            bool isNew = id.Equals(emptyValue);
            bool isValid = repository.Save(isNew, id.Value, viewModel, validationResults);
            if (isValid && validationResults.Count == 0)
            {
                if (getByValueProvider)
                    return new SaveResult(isNew, ValueProvider.GetValue(tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
                else
                    return new SaveResult(isNew, Ax.GetValue(viewModel, tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
            }
            else
                ModelState.AddModelErrors(validationResults);

            return new ErrorResult(ModelState, isUpload);
        }
    }

    #endregion

    #region Default Id Scenarios

    public class BaseScenarios<TViewModel, TRepository> : ThesisBaseController<TRepository> 
        where TRepository : IRepository<TViewModel>
        where TViewModel : class, new()
    {
        string tabTitle, tabId;
        bool getByValueProvider, isUpload;

        public BaseScenarios(TRepository repository, string tabTitle, string tabId): this(repository, tabTitle, tabId, false, false)
        {            
        }

        public BaseScenarios(TRepository repository, string tabTitle, string tabId, bool getByValueProvider) : this(repository, tabTitle, tabId, getByValueProvider, false)
        {            
        }

        public BaseScenarios(TRepository repository, string tabTitle, string tabId, bool getByValueProvider, bool isUpload) : base(repository)
        {
            this.tabTitle = tabTitle;
            this.tabId = tabId;
            this.getByValueProvider = getByValueProvider;
            this.isUpload = isUpload;
        }

        [View]
        [HttpGet]
        [CompressFilter]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public virtual ActionResult List()
        {
            return View();
        }

        [View]
        [HttpPost]
        public virtual ActionResult GetByFilter(GridPanelViewModel model)
        {
            int totalCount;
            var data = repository.GetByFilter(model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }

        [Delete]
        [HttpPost]
        public virtual ActionResult Delete(List<int> rows)
        {
            return new DeleteResult(repository.Delete(rows), rows.Count);
        }

        [View]
        [HttpGet]
        [CompressFilter]
        public virtual ActionResult Detail(int id)
        {
            var model = id == 0 ?
                        new TViewModel() :
                        repository.LoadViewModel(id);

            return View(model ?? new TViewModel());
        }

        [Save]
        [HttpPost]
        public virtual ActionResult Save(int id)
        {
            var viewModel = new TViewModel();

            TryUpdateModel(viewModel, ControllerContext, ModelState);

            var validationResults = new List<RuleViolation>();
            bool isValid = repository.Save(id, viewModel, validationResults);
            if (isValid && validationResults.Count == 0)
            {
                if (getByValueProvider)
                    return new SaveResult(id.Equals(0), ValueProvider.GetValue(tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
                else
                    return new SaveResult(id.Equals(0), Ax.GetValue(viewModel, tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
            }
            else
                ModelState.AddModelErrors(validationResults);

            return new ErrorResult(ModelState, isUpload);
        }
    }

    public class BaseListScenarios<TRepository> : ThesisBaseController<TRepository> 
        where TRepository : IListRepository
    {
        public BaseListScenarios(TRepository repository) : base(repository)
        { }

        [View]
        [HttpGet]
        [CompressFilter]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public virtual ActionResult List()
        {
            return View();
        }

        [View]
        [HttpPost]
        public virtual ActionResult GetByFilter(GridPanelViewModel model)
        {
            int totalCount;
            var data = repository.GetByFilter(model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }

        [Delete]
        [HttpPost]
        public virtual ActionResult Delete(List<int> rows)
        {
            return new DeleteResult(repository.Delete(rows), rows.Count);
        }
    }

    public class BaseDetailScenarios<TViewModel, TRepository> : ThesisBaseController<TRepository>
        where TRepository : IDetailRepository<TViewModel>
        where TViewModel : class, new()
    {
        string tabTitle, tabId;
        bool getByValueProvider, isUpload;

        public BaseDetailScenarios(TRepository repository, string tabTitle, string tabId) : this(repository, tabTitle, tabId, false, false)
        {
        }

        public BaseDetailScenarios(TRepository repository, string tabTitle, string tabId, bool getByValueProvider) : this(repository, tabTitle, tabId, getByValueProvider, false)
        {
        
        }

        public BaseDetailScenarios(TRepository repository, string tabTitle, string tabId, bool getByValueProvider, bool isUpload) : base(repository)
        {
            this.tabTitle = tabTitle;
            this.tabId = tabId;
            this.getByValueProvider = getByValueProvider;
            this.isUpload = isUpload;
        }

        [Delete]
        [HttpPost]
        public virtual ActionResult Delete(List<int> rows)
        {
            return new DeleteResult(repository.Delete(rows), rows.Count);
        }

        [View]
        [HttpGet]
        [CompressFilter]
        public virtual ActionResult Detail(int id)
        {
            var model = id == 0 ?
                        new TViewModel() :
                        repository.LoadViewModel(id);

            return View(model ?? new TViewModel());
        }

        [Save]
        [HttpPost]
        public virtual ActionResult Save(int id)
        {
            var viewModel = new TViewModel();

            TryUpdateModel(viewModel, ControllerContext, ModelState);

            var validationResults = new List<RuleViolation>();
            bool isValid = repository.Save(id, viewModel, validationResults);
            if (isValid && validationResults.Count == 0)
            {
                if (getByValueProvider)
                    return new SaveResult(id.Equals(0), ValueProvider.GetValue(tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
                else
                    return new SaveResult(id.Equals(0), Ax.GetValue(viewModel, tabTitle), Ax.GetValue(viewModel, tabId), isUpload);
            }
            else
                ModelState.AddModelErrors(validationResults);

            return new ErrorResult(ModelState, isUpload);
        }
    }

    #endregion
}
