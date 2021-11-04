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
    
        public const string ConfirmRegister = @"
	                             <div style='padding: 5px;
                                        width: 100%;
                                        display: flex;
                                        flex-direction: column;
                                        text-align: center;
                                        background-color: azure;
                                        overflow-x: hidden;'>
                                    <p style:'font-size: 1.8em;'>Esto es un correo enviado para confirmar el registro en API Blog</p>
                                    <br><br>
                                    <a href='{MI_TEXTO}' target='_blank' style='font-size: 1.5em;
                                        margin: auto;
                                        width: 280px; 
                                        padding: 5px; 
                                        border-radius: 10px;
                                        color: white;
                                        background: blue;
                                        text-decoration: none;
                                        border: 2px solid white;' > Confirmar Registro </a>
                                </div>";
        public const string SendNewPassword = @"
	                             <div style='padding: 5px;
                                        width: 100%;
                                        display: flex;
                                        flex-direction: column;
                                        text-align: center;
                                        background-color: azure;
                                        overflow-x: hidden;'>
                                    <p style:'font-size: 1.8em;'>Su nueva contraseña es :</p>
                                    <br><br>
                                    <button style='font-size: 1.5em;
                                        margin: auto;
                                        width: 280px; 
                                        padding: 5px; 
                                        border-radius: 10px;
                                        color: white;
                                        background: blue;
                                        text-decoration: none;
                                        border: 2px solid white;' > {MI_TEXTO} </button>
                                </div>";
    }
}
