﻿using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Web.RestApi.Models.Entity;
using gBanker.Web.ViewModels;
using log4net.Util.TypeConverters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class FileHandlerController : BaseController
    {
        private readonly IFileService fileUploadService;

        public FileHandlerController(IFileService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        [HttpGet]
        public JsonResult GetDocuments(string Ids)
        {
            try
            {
                if (!string.IsNullOrEmpty(Ids))
                {
                    var idArray = Ids.Split(',');
                    var idArrayLong = idArray.Select(x => long.Parse(string.IsNullOrEmpty(x) ? "0" : x)).ToList();
                    var files = fileUploadService.GetByListOfIds(idArrayLong);
                    return Json(new { Result = "OK", Data = files.Select(x => new
                    {
                        FileName = x.FileName,
                        Type = x.Type,
                        FileUploadId = x.FileUploadId
                    }) }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = "OK", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpGet]
        public JsonResult GetDocument(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    var file = fileUploadService.GetByIdLong(long.Parse(Id));
                    return Json(new
                    {
                        Result = "OK",
                        Data = file
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = "OK", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        public JsonResult UploadDocuments(List<SupportingDocumentUploadModel> files)
        {
            try
            {
                var fileUploadModel = new List<FileUploadTable>();
                foreach(var file in files)
                {
                    var base64FileInfo = GetFileDetails(file.File);
                    fileUploadModel.Add(new FileUploadTable
                    {
                        Type = base64FileInfo.MimeType,
                        FileName = file.FileName,
                        PropertyName = file.FileName,
                        File = base64FileInfo.DataBytes,
                    });
                }
                fileUploadModel = fileUploadService.CreateMany(fileUploadModel).ToList();
                return Json(new { Result = "OK", Data = fileUploadModel }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        public JsonResult UpdateDocument(FileViewModel supportingDocumentUploadModel, long fileId)
        {
            try
            {

                var file = fileUploadService.GetByIdLong(fileId);
                var base64FileInfo = GetFileDetails(supportingDocumentUploadModel.File);
                file.FileName = supportingDocumentUploadModel.FileName;
                file.PropertyName = supportingDocumentUploadModel.PropertyName;
                file.Type = base64FileInfo.MimeType;
                file.File = base64FileInfo.DataBytes;
                file.EntityName = supportingDocumentUploadModel.EntityName;
                fileUploadService.Update(file);
                return Json(new { Result = "OK", Data = file }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public ActionResult GetImage(Int64? Id)
        {
            try
            {
                if (Id != null)
                {
                    var uploadedImage = fileUploadService.GetByIdLong((long)Id);
                    if (uploadedImage != null)
                        return File(uploadedImage.File, "image/*");
                }
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Base64File GetFileDetails(string url)
        {
            string[] file = url.Split(new Char[] { ':', ';', ',' });
            string mimeType = file[1];
            string data = file[3];
            byte[] dataBytes = Convert.FromBase64String(data);
            return new Base64File
            {
                Data = data,
                MimeType = mimeType,
                DataBytes = dataBytes
            };
        }
    }
}