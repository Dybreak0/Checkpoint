using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MobileJO.API.Utilities;
using MobileJO.Data;
using MobileJO.Data.ViewModels.Common;
using System;
using System.Net;
using System.Net.Http;

namespace MobileJO.API.Controllers
{   
    [AllowAnonymous]
    [Route(Constants.Common.Route)]
    [ApiController]
    public class AttachmentAPIController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public AttachmentAPIController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        ///     Downloads a file from the server and returns it to the frontend as byte array data.
        /// </summary>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Attachment.DownloadAttachment)]
        public HttpResponseMessage DownloadAttachment([FromQuery] AttachmentViewModel model)
        {
            var response = new HttpResponseMessage();

            try
            {
                var rootFolderPath = _hostingEnvironment.ContentRootPath;
                var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;
                var filePath = string.Format(Constants.Attachment.FullFilePath, rootFolderPath, attachmentPath, model.ID, model.AttachmentType);
                var fileName = model.FileName;
                return Helper.DownloadFile(fileName, filePath);
            }
            catch (Exception ex)
            {
                var responseCode = new HttpStatusCode();
                var responseData = new object();

                Helper.GetErrors(ex, out responseCode, out responseData);
                response = Helper.ComposeResponse(responseCode, responseData);
            }

            return response;
        }

        /// <summary>
        ///     Handles web-api calls to Download Image of signature from the server and returns it to the frontend as blob data.
        /// </summary>
        /// <param name="model">Contains Attachments properties</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Attachment.DownloadImage)]
        public HttpResponseMessage DownloadImage([FromQuery] AttachmentViewModel model)
        {
            var response = new HttpResponseMessage();

            try
            {
                var rootFolderPath = _hostingEnvironment.ContentRootPath;
                var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;
                var filePath = string.Format(Constants.Attachment.FullFilePath, rootFolderPath, attachmentPath, model.ID, model.AttachmentType);
                var fileName = model.FileName;
                return Helper.DownloadFile(fileName, filePath);
            }
            catch (Exception ex)
            {
                var responseCode = new HttpStatusCode();
                var responseData = new object();

                Helper.GetErrors(ex, out responseCode, out responseData);
                response = Helper.ComposeResponse(responseCode, responseData);
            }

            return response;
        }
    }
}