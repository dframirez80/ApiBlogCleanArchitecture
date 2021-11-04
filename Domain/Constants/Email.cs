using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class Email
    {
        public const string ChangePassword = "La contraseña se cambio exitosamente";
        public const string NewPassword = "Revise su correo electronico con la nueva contraseña";
        public const string Sent = "Revise su correo electronico para validar el registro";
        public const string Subject = "Correo para validacion de registro";
        public const string Confirm = "Su registro fue confirmado.";
        public const string ConfirmFail = "Su registro no pudo ser confirmado o expiro el tiempo, recomendamos que vuelva a registrarse ";
    }
}
