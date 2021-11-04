using AutoMapper;
using Domain.Constants;
using Domain.Email;
using Domain.Models;
using Domain.Models.Dtos;
using Domain.Repository;
using Domain.Repository.Entities;
using Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public class UsersHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMailService _emailService;
        private readonly ITokenJwt _tokenJwt;

        public UsersHandler(IUnitOfWork uow, IMapper mapper, IMailService emailService, ITokenJwt tokenJwt) {
            _uow = uow;
            _mapper = mapper;
            _emailService = emailService;
            _tokenJwt = tokenJwt;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() {
            return await _uow.Users.GetAllUsersAsync();
        }
        public async Task<User> GetUserCompleteAsync(int id) {
            return await _uow.Users.GetUserWithAllDataAsync(id);
        }
        public async Task<User> GetUserExistsAsync(int id) {
            var exists = await _uow.Users.UserExistsAsync(id);
            if (exists)
                return await _uow.Users.GetUserAsync(id);
            return null;
        }
        public async Task DeleteUserAsync(int id) {
            var exists = await _uow.Users.UserExistsAsync(id);
            if (exists) { 
                await _uow.Users.DeleteUserAsync(id);
                await _uow.CommitAsync();
            }
        }
        public async Task UpdateUserAsync(int id, User user) {
            var exists = await _uow.Users.UserExistsAsync(id);
            if (exists)
            {
                await _uow.Users.UpdateUserAsync(user);
                await _uow.CommitAsync();
            }
        }
        public async Task<User> UpdateUserBlockedAsync(int id) {
            var user = await _uow.Users.GetUserAsync(id);
            if (user != null) { 
                user.StatusUser = (int)Enumerations.StatusUser.Blocked;
                await _uow.CommitAsync();
            }
            return user;
        }
        public async Task<User> UpdateUserPendingAsync(int id) {
            var user = await _uow.Users.GetUserAsync(id);
            if (user != null)
            {
                user.StatusUser = (int)Enumerations.StatusUser.Pending;
                await _uow.CommitAsync();
            }
            return user;
        }
        public async Task<User> UpdateUserActiveAsync(int id) {
            var user = await _uow.Users.GetUserAsync(id);
            if (user != null)
            {
                user.StatusUser = (int)Enumerations.StatusUser.Active;
                await _uow.CommitAsync();
            }
            return user;
        }
        public async Task<string> ConfirmUserAsync(int id, string token) {
            var result = _tokenJwt.ValidateToken(token);
            if (id != result)
                return Domain.Constants.Email.ConfirmFail;
            var user = await _uow.Users.GetUserAsync(id);
            if (user == null)
                return Domain.Constants.Email.ConfirmFail;
            user.StatusUser = (int)Enumerations.StatusUser.Active;
            await _uow.CommitAsync();
            return Domain.Constants.Email.Confirm;
        }
        public async Task<string> RegisterUserAsync(UserDto userDto, string host) {
            if (userDto == null)
                return string.Empty;
            var user = await _uow.Users.UserExistsAsync(userDto.Email);              // verifica si existe usuario x email
            if (user != null)
            {
                if (user.StatusUser == (int)Enumerations.StatusUser.Active)       // verifica si usuario esta activo
                    return ErrorMessage.UserExists;
                if (user.StatusUser == (int)Enumerations.StatusUser.Blocked)       // verifica si usuario esta bloqueado
                    return ErrorMessage.UserBlocked;
            }

            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);    // cifrar contraseña
            if (user != null)
            {     // si esta registrado con estado pendiente
                int userId = user.UserId;
                user = _mapper.Map<User>(userDto);                                  // actualizo DB
                user.UserId = userId;
            } else
            {
                user = _mapper.Map<User>(userDto);
                user.Role = Roles.User;
                user.StatusUser = (int)Enumerations.StatusUser.Pending;
                user.Created = DateTime.UtcNow.AddHours(UTC.GmtBuenosAires);
                await _uow.Users.CreateUserAsync(user);                              // crea usuario con estado pendiente
            }
            await _uow.CommitAsync();

            // envio de correo
            var token = _tokenJwt.GenerateToken(user, TokenExpires.Register);
            string path = $"https://{host}/api/v1/users/confirm/{user.UserId}/{token}";
            EmailRequest emailRequest = new() { ToEmail = user.Email, Subject = Domain.Constants.Email.Subject, Body = path };
            await _emailService.EmailConfirmRegister(emailRequest);

            return Domain.Constants.Email.Sent;
        }
        public async Task<string> ChangeUserPasswordAsync(ChangePassword changePassword) {
            var user = await _uow.Users.UserExistsAsync(changePassword.Email);              // verifica si existe usuario x email
            if (user == null)
                return ErrorMessage.EmailOrPassword;
            if (BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);    // cifrar contraseña
                user.ResetPassword = false;
                await _uow.CommitAsync();
                return Domain.Constants.Email.ChangePassword;
            }
            return ErrorMessage.EmailOrPassword;
        }
        public async Task<string> ResetUserPasswordAsync(ResetPassword email, string host) {
            var user = await _uow.Users.UserExistsAsync(email.Email);                              // verifica password y correo
            if (user == null)
                return ErrorMessage.UserNotLogin;
            Random r = new();
            var newPass = r.Next(10000000, 90000000);
            string pass = newPass.ToString();
            user.Password = BCrypt.Net.BCrypt.HashPassword(pass);                      // cifrar contraseña
            user.ResetPassword = true;
            await _uow.CommitAsync();

            var token = _tokenJwt.GenerateToken(user, TokenExpires.Register);
            EmailRequest emailRequest = new() { ToEmail = user.Email, Subject = Domain.Constants.Email.Subject, Body = newPass.ToString() };
            await _emailService.EmailChangePassword(emailRequest);
            return Domain.Constants.Email.NewPassword;
        }
        public async Task<string> RegisterAdminAsync(UserDto userDto) {
            var userExists = await _uow.Users.UserExistsAsync(userDto.Email);   //verifica si existe email
            if (userExists != null)
                return ErrorMessage.UserExists;
            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);    // cifrar contraseña
            var user = _mapper.Map<User>(userDto);
            user.Role = Roles.Admin;
            user.StatusUser = (int)Enumerations.StatusUser.Active;
            user.Created = DateTime.UtcNow.AddHours(UTC.GmtBuenosAires);
            await _uow.Users.CreateUserAsync(user);
            await _uow.CommitAsync();
            return user.UserId.ToString();
        }
        public async Task<string> LoginUserAsync(Login login) {
            var user = await _uow.Users.UserExistsAsync(login.Email);                              // verifica password y correo
            if (user == null)
                return ErrorMessage.UserNotLogin;
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return ErrorMessage.UserNotLogin;
            if (user.StatusUser == (int)Enumerations.StatusUser.Active)
            {
                if (!user.ResetPassword){
                    var tokenUser = _tokenJwt.GenerateToken(user, TokenExpires.Login);          // generar token
                    return tokenUser;
                } 
                return ErrorMessage.ResetPassword;
            }
            if (user.StatusUser == (int)Enumerations.StatusUser.Pending)
                return ErrorMessage.UserPending;
            else
                return ErrorMessage.UserBlocked;
        }
    }


}
