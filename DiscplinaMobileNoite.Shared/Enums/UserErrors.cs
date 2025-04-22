using System.ComponentModel;

namespace DiscplinaMobileNoite.Shared.Enums
{
    public enum UserErrors
    {
        [Description("'Email' can not be null or empty!")]
        User_Error_EmailCanNotBeNullOrEmpty,

        [Description("'Email' invalid format!")]
        User_Error_InvalidEmailFormat,

        [Description("'Email' can not be less 4 letters!")]
        User_Error_EmailLenghtLessFour,

        [Description("'FullName' can not be null or empty!")]
        User_Error_FullNameCanNotBeNullOrEmpty,

        [Description("'FullName' can not be less 4 letters!")]
        User_Error_FullNameLenghtLessFour,

        [Description("'Workload' can not be null or empty!")]
        User_Error_WorkloadCanNotBeNullOrEmpty,

        [Description("'Workload' must be greater than zero!")]
        User_Error_WorkloadMustBeGreaterThanZero,

        [Description("'Password' can not be null or empty!")]
        User_Error_PasswordCanNotBeNullOrEmpty,

        [Description("'PhoneNumber' can not be null or empty!")]
        User_Error_PhoneNumberCanNotBeNullOrEmpty,
    }
}