using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Web.Core
{
    public class UploadCallbackResult
    {
        public UploadCallbackResult(Error error, string successMessage)
        {
            Error = error;
            SuccessMessage = successMessage;
        }

        public Error Error { get; internal set; }

        public string SuccessMessage { get; internal set; }
    }

    public class UploadCallbackResult<T> : UploadCallbackResult
    {
        public UploadCallbackResult(T data, string FileName = null, string StoredFileName = null, string FileSize = null, 
            Error error = null, string successMessage = null) : base(error, successMessage)
        {
            Data = data;
        }

        public T Data { get; internal set; }

        public string FileName { get; set; }

        public string StoredFileName { get; set; }

        public string FileSize { get; set; }

        public static UploadCallbackResult<T> CreateFrom<Y>(UploadCallbackResult<Y> source) where Y : T
        {
            return new UploadCallbackResult<T>((T)source.Data, source.FileName, source.StoredFileName, source.FileSize,
                source.Error, source.SuccessMessage);
        }
    }

}
