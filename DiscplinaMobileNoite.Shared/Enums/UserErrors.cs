using System.ComponentModel;

namespace DiscplinaMobileNoite.Shared.Enums
{
    public enum UserErrors
    {
        [Description("Preencha o campo email!")]
        User_Error_EmailCanNotBeNullOrEmpty,

        [Description("Email no formato inválido!")]
        User_Error_InvalidEmailFormat,

        [Description("O campo email não pode ter menos que 4 letras!")]
        User_Error_EmailLenghtLessFour,

        [Description("Preencha o campo nome completo!")]
        User_Error_FullNameCanNotBeNullOrEmpty,

        [Description("O campo nome completo não pode ter menos que 4 letras!")]
        User_Error_FullNameLenghtLessFour,

        [Description("Preencha o campo carga horária!")]
        User_Error_WorkloadCanNotBeNullOrEmpty,

        [Description("O campo carga horária não pode ter menos que 0 horas!")]
        User_Error_WorkloadMustBeGreaterThanZero,

        [Description("Preencha o campo senha!")]
        User_Error_PasswordCanNotBeNullOrEmpty,

        [Description("Preencha o campo número de telefone!")]
        User_Error_PhoneNumberCanNotBeNullOrEmpty,
    }
}