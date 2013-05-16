using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Enumerations;

namespace Thesis.Common.ViewModels
{
    public class GridPanelViewModel : IDataViewModel, IExportViewModel
    {
        #region IDataViewModel Members

        public int Start { get; set; }
        public int Limit { get; set; }
        public string Dir { get; set; }
        public string Sort { get; set; }
        public List<FilterViewModel> Filter { get; set; }
        public List<FilterViewModel> FilterBase { get; set; }

        #endregion

        #region IExportViewModel Members

        public bool IsExport { get; set; }
        public string ExportFormat { get; set; }

        #endregion
    }

    public interface IDataViewModel
    {
        int Start { get; set; }
        int Limit { get; set; }
        string Dir { get; set; }
        string Sort { get; set; }
        List<FilterViewModel> Filter { get; set; }
        List<FilterViewModel> FilterBase { get; set; }
    }

    public interface IExportViewModel
    {
        bool IsExport { get; set; }
        string ExportFormat { get; set; }
    }

    public class FilterViewModel
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public PredicateCondition WhereCondition { get; set; }
        public FilterCondition FilterCondition { get; set; }
        public string StartValue { get; set; }
        public string EndValue { get; set; }
    }
}
