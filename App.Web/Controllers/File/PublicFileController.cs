using App.Core.Authorization;
using App.Core.FileManagement;
using App.Core.FileManagement.Files;
using App.Core.FileManagement.Files.Dto;
using App.Core.FileManagement.PublicFiles;
using App.Core.FileManagement.PublicFiles.Dto;
using Microsoft.AspNetCore.Mvc;
using App.Web.FileUrl;
using App.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace App.Web.Controllers.Files
{
    /// <summary>
    /// 文件管理-公共文件
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public-files")]
    public class PublicFileController: ControllerBase
    {
        private IPublicFileService _fileService;
        public PublicFileController(
            IPublicFileService fileService
            )
        {
            _fileService = fileService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("")]
        public List<GetPublicFileOutput> Upload()
        {
            var files = _fileService.UploadFiles(Request.Form.Files);
            files.ForEach(file =>
            {
                file.Url = $"{AppWebContext.Instance.Domain}/api/v1/public-files/{file.Id}/data";
            });
            return files;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("images")]
        public List<GetPublicFileOutput> UploadImages()
        {
            var files = _fileService.UploadFiles(
                Request.Form.Files, 
                new FileExtensions[] {
                    FileExtensions.JPG,
                    FileExtensions.PNG,
                    FileExtensions.BMP
                }
                );
            files.ForEach(file =>
            {
                file.Url = $"{AppWebContext.Instance.Domain}/api/v1/public-files/{file.Id}/data";
            });
            return files;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/data")]
        public IActionResult Download(int id)
        {
            (var filename, var contentType, var filepath) = _fileService.GetFile(id);
            return PhysicalFile(filepath, contentType, filename);
        }
    }
}