using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class ErrorMessage
    {
        public const string UserId = "Debe ingresar Id de usuario";
        public const string Names = "Debe ingresar nombre";
        public const string Surnames = "Debe ingresar Apellido";
        public const string Email = "Debe ingresar un correo valido";
        public const int MaxLengthPassword = 18;
        public const int MinLengthPassword = 6;
        public const string Title = "Debe ingresar titulo";
        public const string Keyword = "Debe ingresar palabras claves";
        public const string Content = "Debe ingresar contenido";

        public const string EmailOrPassword = "Debe ingresar un correo/contraseña valido";
        public const string UserExists = "El correo ya existe.";
        public const string UserBlocked = "El usuario se encuentra bloquedao, contacte al Administrador.";
        public const string UserNotLogin = "El correo o la contraseña no es valida.";
        public const string UserPending = "El correo todavia no fue verificado, revise su correo en la carpeta de spam.";
        public const string ResetPassword = "Debe cambiar la contraseña";
        public const string InfoInvalid = "La informacion es invalida";

    }
}
