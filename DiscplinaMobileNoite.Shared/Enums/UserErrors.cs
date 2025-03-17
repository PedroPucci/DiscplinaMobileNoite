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
    }
}