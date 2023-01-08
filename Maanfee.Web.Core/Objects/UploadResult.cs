namespace Maanfee.Web.Core
{
    // ==========================Should delete 
    public class UploadResult
    {
        public int ErrorCode { get; set; }

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        public bool Uploaded { get; set; }

        public string FileName { get; set; }

        public string StoredFileName { get; set; }
    }

    public class UploadResult<T>
    {
        //public UploadResult(bool Uploaded, string FileName, string StoredFileName, Error error, string successMessage)
        //{
        //    Error = error;
        //    SuccessMessage = successMessage;
        //}

        //public Error Error { get; internal set; }

        public int ErrorCode { get; set; }

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        public bool Uploaded { get; set; }

        public string FileName { get; set; }

        public string StoredFileName { get; set; }

        public T Data { get; set; }


        //public static UploadResult CreateFrom(UploadResult source)
        //{
        //    return new UploadResult(source.Uploaded, source.FileName, source.StoredFileName, source.Error, source.SuccessMessage);
        //}
    }
}
