using Maanfee.Dashboard.Resources;
using Maanfee.Web.Core;

namespace Maanfee.Dashboard.Core
{
    public class MessageHandler
    {
        public static string ErrorHandler(Error error)
        {
            switch (error.Code) 
            {
                case ErrorCode.DeleteError:
                    return DashboardResource.MessageDeleteConstraint;
                case ErrorCode.DuplicateError:
                    return DashboardResource.MessageCannotInsertDuplicate;
                case ErrorCode.InvalidReferralCode:
                    return DashboardResource.MessageInvalidReferralCode;
                case ErrorCode.ItemAlreadyTaken:
                    return DashboardResource.MessageItemAlreadyTaken;
                case ErrorCode.InvalidUserName:
                    return DashboardResource.MessageInvalidUsername;
                case ErrorCode.ChangeIsNotPossible:
                    return DashboardResource.MessageChangeIsNotPossible;
                case ErrorCode.NoMatchingRecordsFound:
                    return DashboardResource.MessageNoMatchingRecordsFound;
                default:
                    return error.ToString();
            }
        }
    }
}
