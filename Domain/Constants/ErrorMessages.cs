using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class ErrorMessages
    {
        public const string UnexpectedErrorMessageTitle = "Unexcepted error";
        public const string UnexpectedErrorMessage = "Unexcepted error happend";

        public const string NoAccessErrorMessageTitle = "No access errorr";
        public const string NoAccessErrorMessage = "you do not have access";

        public const string BadDataErrorMessage = "error bad data";
        public const string ResourceNotFound = "enot founded";

        public const string InvalidToken = "invalid token";

        public const string NotAuthorized = "not authorized";

        public const string EntityAlreadyExists = "sl ready exist";

        public const string NoPermissionToLogin = "you don't have premission";
    }
}
