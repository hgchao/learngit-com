using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.FileUrl
{
    public interface IFileUrlBuilder
    {
        int Decode(string uid);
        void SetAttachFileUrl<T>(T t);
        void SetPublicFileUrl<T>(T t);
        void SetAttachFileUrl2<T>(T t);
        void SetPublicFileUrl2<T>(T t);
    }
}
