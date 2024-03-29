﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Thesis.Lib.Controllers;
using Ext.Net.MVC;
using Shell;
using Thesis.Common.Models;

namespace Thesis.Controllers
{
    public class FilesController : ThesisBaseController<IFilesRepository>
    {
        public FilesController(IFilesRepository repository) 
            : base(repository)
        {

        }

        #region Methods

        private static Ext.Net.Icon GetFileIcon(string extension)
        {
            extension = extension.Trim().ToLower().Replace(".", "");

            string[] images = { "jpg", "jpeg", "gif", "png" };
            if (Array.IndexOf(images, extension) != -1)
                return Ext.Net.Icon.Picture;

            string[] wordDocs = { "doc", "docx" };
            if (Array.IndexOf(wordDocs, extension) != -1)
                return Ext.Net.Icon.PageWord;

            string[] excelDocs = { "xls", "xlsx" };
            if (Array.IndexOf(excelDocs, extension) != -1)
                return Ext.Net.Icon.PageExcel;

            string[] ppDocs = { "ppt", "pptx", "pps", "ppsx" };
            if (Array.IndexOf(ppDocs, extension) != -1)
                return Ext.Net.Icon.PageWhitePowerpoint;

            string[] compress = { "zip", "rar" };
            if (Array.IndexOf(compress, extension) != -1)
                return Ext.Net.Icon.PageWhiteCompressed;

            if (extension == "txt") return Ext.Net.Icon.PageWhiteText;
            if (extension == "pdf") return Ext.Net.Icon.PageWhiteAcrobat;
            if (extension == "xml") return Ext.Net.Icon.PageCode;
            if (extension == "csv") return Ext.Net.Icon.PageAttach;

            return Ext.Net.Icon.Page;
        }

        #endregion

        [HttpPost]
        public ActionResult FileUpload(string allowedFileTypes, int allowedFileSize, string path, string fieldID, int fileID)
        {
            HttpPostedFileBase postedFile = null;
            if (Request.Files.Count > 0)
                postedFile = Request.Files[string.Format("{0}-file", fieldID)];

            if (postedFile == null || postedFile.ContentLength == 0)
            {
                AjaxFormResult formResult = new AjaxFormResult();
                formResult.Success = false;
                formResult.IsUpload = true;
                formResult.Errors.Add(new FieldError(fieldID, "Please select a file"));
                return formResult;
            }

            if (allowedFileSize > decimal.Zero)
            {
                if (postedFile.ContentLength > allowedFileSize)
                {
                    AjaxFormResult formResult = new AjaxFormResult();
                    formResult.Success = false;
                    formResult.IsUpload = true;
                    formResult.Errors.Add(new FieldError(fieldID, string.Format("Please check your file size.Allowed max size : {0} bytes", allowedFileSize)));
                    return formResult;
                }
            }

            string[] fileTypes = null;
            if (!string.IsNullOrEmpty(allowedFileTypes))
                fileTypes = allowedFileTypes.Split(',');

            string fileName = Path.GetFileName(postedFile.FileName);

            if (fileTypes != null)
            {
                bool isAllowedFile = false;
                for (int i = 0; i < fileTypes.Length; i++)
                {
                    if (fileName.ToLower().EndsWith(fileTypes[i].ToLower().Trim()))
                    {
                        isAllowedFile = true;
                        break;
                    }
                }
                if (!isAllowedFile)
                {
                    AjaxFormResult formResult = new AjaxFormResult();
                    formResult.Success = false;
                    formResult.IsUpload = true;
                    formResult.Errors.Add(new FieldError(fieldID, string.Format("Please check your file.Allowed file extensions : {0}", allowedFileTypes)));
                    return formResult;
                }
            }

            if (string.IsNullOrEmpty(path))
                path = Thesis.Common.Helpers.Ax.GetAppSetting("FilesPath");

            string extension = Path.GetExtension(postedFile.FileName);

            FileViewModel file = fileID > 0 ? repository.LoadViewModel(fileID) : new FileViewModel();
            if (file == null) file = new FileViewModel();
            file.Path = Server.MapPath(@"\" + path);
            file.FileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
            file.Mimetype = postedFile.ContentType;
            file.Size = postedFile.ContentLength;
            file.Alias = fileName;

            postedFile.SaveAs(Path.Combine(file.Path, file.FileName));

            var validationResults = new List<RuleViolation>();
            bool isSuccess = repository.Save(fileID, file, validationResults);

            AjaxFormResult response = new AjaxFormResult();
            response.Success = isSuccess;
            response.IsUpload = true;

            if (isSuccess)
            {
                Ext.Net.Icon icon = GetFileIcon(extension);
                response.ExtraParams["Icon"] = icon.ToString();
                response.ExtraParams["IconCls"] = Ext.Net.ResourceManager.GetIconClassName(icon);
                response.ExtraParams["FileName"] = file.Alias;
                response.ExtraParams["FileID"] = file.FileID.ToString();
            }
            else
            {
                validationResults.ForEach(p => response.Errors.Add(new FieldError(p.PropertyName, p.ErrorMessage)));
            }
            return response;
        }

        [HttpPost]
        public ActionResult DeleteFile(List<int> fileIds)
        {
            if (fileIds == null || fileIds.Count == 0) 
                return new AjaxFormResult() { Success = false };

            bool isSuccess = true;
            foreach (var fileId in fileIds)
            {
                var file = repository.LoadViewModel(fileId);
                if (file == null) continue;
                if (repository.Delete(fileId))
                {
                    string fullPath = Path.Combine(file.Path, file.FileName);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
                else
                {
                   if(isSuccess) isSuccess = false;
                }
            }

            return new AjaxFormResult() { Success = isSuccess };
        }

        [HttpPost]
        public ActionResult FileDownload(int id)
        {
            var file = repository.LoadViewModel(id);
            if (file == null) return new EmptyResult();
            string fullPath = Path.Combine(file.Path, file.FileName);
            if (!System.IO.File.Exists(fullPath)) return new EmptyResult();
            FileContentResult contentResult = new FileContentResult(System.IO.File.ReadAllBytes(fullPath), file.Mimetype);
            contentResult.FileDownloadName = file.Alias;
            return contentResult;
        }

        [HttpPost]
        public ActionResult GetFileByID(int id)
        {
            var file = repository.LoadViewModel(id);
            if (file == null) return new AjaxFormResult() { Success = false };

            AjaxFormResult response = new AjaxFormResult();
            string fullPath = Path.Combine(file.Path, file.FileName);
            if (System.IO.File.Exists(fullPath))
            {
                response.Success = true;
                Ext.Net.Icon icon = GetFileIcon(Path.GetExtension(fullPath));
                response.ExtraParams["Icon"] = icon.ToString();
                response.ExtraParams["IconCls"] = Ext.Net.ResourceManager.GetIconClassName(icon);
                response.ExtraParams["FileName"] = file.Alias;
                response.ExtraParams["FileID"] = file.FileID.ToString();
            }
            else
                response.Success = false;

            return response;
        }      
    }
}
