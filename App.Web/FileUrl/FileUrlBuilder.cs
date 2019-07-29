using App.Core.Authorization;
using App.Core.Authorization.Accounts;
using PoorFff.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using App.Core.FileManagement.AttachmentFileMetas;
using App.Core.FileManagement.Files;

namespace App.Web.FileUrl
{
    public class FileUrlBuilder: IFileUrlBuilder
    {
        private IAuthInfoProvider _authInfoProvider;
        public FileUrlBuilder(IAuthInfoProvider authInfoProvider)
        {
            _authInfoProvider = authInfoProvider;
        }
        public int Decode(string uid)
        {
            var info = AppWebContext.Instance.Cache.Get<StoreInfo>(uid);
            _authInfoProvider.SetCurrent(info.AuthInfo);
            return info.ObjectId;
        }

        public void SetAttachFileUrl<T>(T t)
        {
            AttachmentFileMetaOutputTool.AddUrl(t, fileMetaId => {
                return Build(fileMetaId);
            });
        }
        public void SetAttachFileUrl2<T>(T t)
        {
            FileMetaOutputTool.AddUrl(t, fileMetaId => {
                return Build(fileMetaId);
            });
        }

        public void SetPublicFileUrl<T>(T t)
        {
            AttachmentFileMetaOutputTool.AddUrl(t, fileMetaId => {
                return $"{AppWebContext.Instance.Domain}/api/v1/public-files/{fileMetaId}/data";
            });
        }

        public void SetPublicFileUrl2<T>(T t)
        {
            FileMetaOutputTool.AddUrl(t, fileMetaId => {
                return $"{AppWebContext.Instance.Domain}/api/v1/public-files/{fileMetaId}/data";
            });
        }

        private string Build(int id, int version = 1, string controller = "files")
        {
            var uid = Guid.NewGuid().ToString("N");
            var info = new StoreInfo { ObjectId = id, AuthInfo = _authInfoProvider.GetCurrent() };
            AppWebContext.Instance.Cache.Add(uid, info, PoorFff.Cache.ExpiredTimeType.FixedExpiration, new TimeSpan(5, 0, 0));
            return $"{AppWebContext.Instance.Domain}/api/v{version}/{controller}/{uid}/data";
        }

    }
    public class StoreInfo
    {
        public int ObjectId { get; set; }
        public AuthInfo AuthInfo { get; set; }
    }
}
