﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uof;

        public UserService(IUnitOfWork uof)
        {
            _uof = uof;
        }
      

        public async Task<OperationDetails> Register(UserDto userDto)
        {
            try
            {
                if (_uof.Users.Entities.Any(u => u.UserName==userDto.UserName)) throw new Exception("Пользователь с таким именем уже существует");
                var user = new User
                {
                    UserName = userDto.UserName, Password = userDto.Password, RoleId = 2,
                    Image = userDto.Image, MimeType = userDto.MimeType
                };
                _uof.Users.Create(user);
                await _uof.Commit();
                return new OperationDetails(true,$"Пользователь {userDto.UserName} удачно зарегестрирован", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new OperationDetails(false, "Регистрация не прошла:" , e.Message);
            }
        }

        public async Task<OperationDetails> EditUser(UserDto userDto)
        {
            try
            {
                var user = await _uof.Users.GetAsync(2);
                user.UserName = userDto.UserName;
                user.Password = userDto.Password;
                user.Image = userDto.Image;
                user.MimeType = userDto.MimeType;
                
                _uof.Users.Update(user);
                await _uof.Commit();
                return new OperationDetails(true, $"Пользователь {userDto.UserName} удачно зарегестрирован", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new OperationDetails(false, "Регистрация не прошла:", e.Message);
            }
        }
    }
}