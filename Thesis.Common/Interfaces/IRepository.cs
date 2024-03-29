﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Objects.DataClasses;
using Thesis.Common.ViewModels;
using Thesis.Common.Models;

namespace Thesis.Common.Interfaces
{
    #region Generic Id Type

    /// <summary>
    /// LoadViewModel, Save, Delete ve GetByFilter methodlarını içerir
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel tipi</typeparam>
    /// <typeparam name="TIdType">Id tipi</typeparam>
    public interface IRepository<TViewModel, TIdType> where TViewModel : class, new()
    {
        TViewModel LoadViewModel(TIdType id);
        bool Save(bool isNew, TIdType id, TViewModel viewModel, List<RuleViolation> validationResults);
        bool Delete(List<TIdType> models);
        IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount);
    }

    /// <summary>
    /// Delete ve GetByFilter methodlarını içerir
    /// </summary>
    /// <typeparam name="TIdType">Id tipi</typeparam>
    public interface IListRepository<TIdType>
    {
        bool Delete(List<TIdType> models);
        IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount);
    }

    /// <summary>
    /// LoadViewModel, Save, Delete methodlarını içerir
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel tipi</typeparam>
    /// <typeparam name="TIdType">Id tipi</typeparam>
    public interface IDetailRepository<TViewModel, TIdType> where TViewModel : class, new()
    {
        TViewModel LoadViewModel(TIdType id);
        bool Save(bool isNew, TIdType id, TViewModel model, List<RuleViolation> validationResults);
        bool Delete(List<TIdType> models);
    }

    #endregion

    #region Default Id Type

    /// <summary>
    /// LoadViewModel, Save, Delete ve GetByFilter methodlarını içerir, default id tipi int
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel tipi</typeparam>
    public interface IRepository<TViewModel> where TViewModel : class, new()
    {
        TViewModel LoadViewModel(int id);
        bool Save(int id, TViewModel viewModel, List<RuleViolation> validationResults);
        bool Delete(List<int> models);
        IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount);
    }

    /// <summary>
    /// Delete ve GetByFilter methodlarını içerir, default id tipi int
    /// </summary>
    public interface IListRepository
    {
        bool Delete(List<int> models);
        IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount);
    }

    /// <summary>
    /// LoadViewModel, Save ve Delete methodlarını içerir, default id tipi int
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel tipi</typeparam>
    public interface IDetailRepository<TViewModel> where TViewModel : class, new()
    {
        TViewModel LoadViewModel(int id);
        bool Save(int id, TViewModel viewModel, List<RuleViolation> validationResults);
        bool Delete(List<int> models);
    }

    #endregion
}
