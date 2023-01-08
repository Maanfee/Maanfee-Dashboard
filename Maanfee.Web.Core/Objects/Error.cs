namespace Maanfee.Web.Core
{
    public enum ErrorCode
    {
        SystemError = 100,
        DuplicateError = 200,
        DeleteError = 300,
         
        FileSizeIsZero = 1,
        FileSizeIsLarge = 2,
        FileCount = 3,
        FileNotUploadedOnServer = 4,
        FileNotUploaded = 5,
        FileUnknowError = 6,
        FileMaximumNumberOfFiles = 7,

        InvalidReferralCode = 101,
        ItemAlreadyTaken = 102,
        InvalidUserName = 103,
        ChangeIsNotPossible = 104,
        NoMatchingRecordsFound = 105,

        PrimaryKeyConstraint = 201,
    }

    public class Error
    {
        public Error(ErrorCode? code, string message, object data = null)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public ErrorCode? Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public override string ToString()
        {
            return $"Error {(int)Code} {Code}: {Message}";
        }
    }

    public class ExceptionError : Error
    {
        public ExceptionError(string message, object data = null) : base(ErrorCode.SystemError, message, data) { }
    }

    public class DuplicateError : Error
    {
        public DuplicateError(string message, object data = null) : base(ErrorCode.DuplicateError, message, data) { }
    }

    public class PrimaryKeyConstraintError : Error
    {
        public PrimaryKeyConstraintError(string message, object data = null) : base(ErrorCode.PrimaryKeyConstraint, message, data) { }
    }

    public class DeleteError : Error
    {
        public DeleteError(string message, object data = null) : base(ErrorCode.DeleteError, message, data) { }
    }
}
