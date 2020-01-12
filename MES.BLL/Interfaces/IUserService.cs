using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> Register(UserDto userDto);
        Task<OperationDetails> EditUser(UserDto userDto);
        Task<IEnumerable<UserDto>> GetUsers();
        Task<OperationDetails> DeleteUser(int id);
    }
}