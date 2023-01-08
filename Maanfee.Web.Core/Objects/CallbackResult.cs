namespace Maanfee.Web.Core
{
    public class CallbackResult
    {
        public CallbackResult(Error error, string successMessage)
        {
            Error = error;
            SuccessMessage = successMessage;
        }

        public Error Error { get; internal set; }

        public string SuccessMessage { get; internal set; }
    }

    public class CallbackResult<T> : CallbackResult
    {
        public CallbackResult(T data, Error error, string successMessage = null) : base(error, successMessage)
        {
            Data = data;
        }

        public T Data { get; internal set; }

        public static CallbackResult<T> CreateFrom<Y>(CallbackResult<Y> source) where Y : T
        {
            return new CallbackResult<T>((T)source.Data, source.Error, source.SuccessMessage);
        }
    }
}
