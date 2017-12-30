using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace MES.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            var user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = Mapper.Map<ApplicationUser>(userDto);
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента
                var clientProfile = new ClientProfile { Id = user.Id, Address = userDto.Address, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                await Database.Commit();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            var user = await Database.UserManager.FindAsync(userDto.Name, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            //foreach (var roleName in roles)
            //{
            //    var role = await Database.RoleManager.FindByNameAsync(roleName);
            //    if (role != null) continue;
            //    role = new ApplicationRole { Name = roleName };
            //    await Database.RoleManager.CreateAsync(role);
            //}
            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
