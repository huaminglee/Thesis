using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using Ext.Net.MVC;
using Thesis.Common.ViewModels;

namespace Thesis.Lib.ActionResults
{
    public class GridPanelResult : ActionResult
    {
        private IEnumerable data;
        private int totalCount;
        private bool isExport;
        private string exportFormat;

        public GridPanelResult(IEnumerable data, int totalCount)
        {
            this.data = data;
            this.totalCount = totalCount;
        }

        public GridPanelResult(IEnumerable data, int totalCount, IExportViewModel viewModel)
        {
            this.data = data;
            this.totalCount = totalCount;
            this.isExport = viewModel.IsExport;
            this.exportFormat = viewModel.ExportFormat;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (isExport)
            {
                ExportResult exportResult = new ExportResult(data, exportFormat);
                exportResult.ExecuteResult(context);
            }
            else
            {
                AjaxStoreResult storeResult = new AjaxStoreResult(data, totalCount);
                storeResult.ExecuteResult(context);
            }
                
        }
    }
}
