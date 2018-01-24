using MES.BLL.Interfaces;
using MES.DAL.EF;

namespace MES.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService()
        {
            return new UserService(new UnitOfWork());
        }
    }
}