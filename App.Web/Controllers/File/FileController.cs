using App.Core.Authorization;
using App.Core.FileManagement;
using App.Core.FileManagement.Files;
using App.Core.FileManagement.Files.Dto;
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
using static System.Net.WebRequestMethods;

namespace App.Web.Controllers.Files
{
    /// <summary>
    /// 文件管理-私有文件
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/files")]
    public class FileController: ControllerBase
    {
        private IFileService _fileService;
        private IFileUrlBuilder _fileUrlBuilder;
        public FileController(
            IFileService fileService,
            IFileUrlBuilder fileUrlBuilder
            )
        {
            _fileService = fileService;
            _fileUrlBuilder = fileUrlBuilder;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("")]
        public List<GetFileMetaOutput> Upload()
        {
            var files = _fileService.UploadFiles(Request.Form.Files, "All");
            _fileUrlBuilder.SetAttachFileUrl2(files);
            return files;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost("images")]
        public List<GetFileMetaOutput> UploadImages()
        {
            var files = _fileService.UploadFiles(
                Request.Form.Files, 
                "Image",
                new FileExtensions[] {
                    FileExtensions.JPG,
                    FileExtensions.PNG,
                    FileExtensions.BMP
                }
                );
            _fileUrlBuilder.SetAttachFileUrl2(files);
            return files;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("{uid}/data")]
        public IActionResult Download(string uid)
        {
            int id = _fileUrlBuilder.Decode(uid);
            if(id <= 0)
            {
                return NotFound("找不到对应图片");
            }
            (var filename, var contentType, var filepath) = _fileService.GetFile(id);
            return PhysicalFile(filepath, contentType, filename);
        }

        /// <summary>
        /// 获取所有文件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<GetFileMetaOutput> GetAll()
        {
            return _fileService.GetAll();
        }
    }
}