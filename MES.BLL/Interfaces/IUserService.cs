using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> Register(UserDto userDto);
    }
}