using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
    }
}