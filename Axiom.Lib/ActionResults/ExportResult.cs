using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using Ext.Net;
using System.Xml;
using System.Xml.Xsl;
using System.Web;

namespace Thesis.Lib.ActionResults
{
    public class ExportResult : ActionResult
    {
        private string exportFormat { get; set; }
        private IEnumerable data { get; set; }

        public ExportResult(IEnumerable data, string exportFormat)
        {
            this.exportFormat = exportFormat.ToLower().Trim();
            this.data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            //excel -> icon-pageexcel
            //xml -> icon-pagecode
            //csv -> icon-pageattach

            FileContentResult contentResult = null;

            string jsonData = JSON.Serialize(data);

            if (exportFormat == "icon-pagecode")
                jsonData = jsonData.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

            SubmitHandler handler = new SubmitHandler(jsonData);
            XmlNode xmlData = handler.Xml;

            switch (exportFormat)
            {
                case "icon-pageexcel":
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(HttpContext.Current.Server.MapPath("~/Content/ExportTemplate/Excel.xsl"));
                    StringBuilder resultExcelString = new StringBuilder();
                    XmlWriter excelWriter = XmlWriter.Create(resultExcelString);
                    xtExcel.Transform(xmlData, excelWriter);

                    contentResult = new FileContentResult(Encoding.UTF8.GetBytes(resultExcelString.ToString()), "application/vnd.ms-excel");
                    contentResult.FileDownloadName = DateTime.Now.ToString() + ".xls";
                    break;
                case "icon-pagecode":
                    contentResult = new FileContentResult(Encoding.UTF8.GetBytes(xmlData.OuterXml), "application/xml");
                    contentResult.FileDownloadName = DateTime.Now.ToString() + ".xml";
                    break;
                case "icon-pageattach":
                    XslCompiledTransform xtCsv = new XslCompiledTransform();
                    xtCsv.Load(HttpContext.Current.Server.MapPath("~/Content/ExportTemplate/Csv.xsl"));
                    StringBuilder resultCsvString = new StringBuilder();

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.OmitXmlDeclaration = true;
                    settings.ConformanceLevel = ConformanceLevel.Fragment;
                    settings.CloseOutput = false;

                    XmlWriter csvWriter = XmlWriter.Create(resultCsvString, settings);
                    xtCsv.Transform(xmlData, csvWriter);

                    contentResult = new FileContentResult(Encoding.UTF8.GetBytes(resultCsvString.ToString()), "application/octet-stream");
                    contentResult.FileDownloadName = DateTime.Now.ToString() + ".csv";
                    break;
                default:
                    break;
            }

            if(contentResult != null)
                contentResult.ExecuteResult(context);
        }
    }
}
