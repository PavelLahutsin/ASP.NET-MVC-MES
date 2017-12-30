using System;
using System.Threading.Tasks;
using MES.DAL.Entities;
using MES.DAL.Identity;

namespace MES.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ArrivalOfDetail> ArrivalOfDetails { get; }
        IBaseRepository<Detail> Details { get; }
        IBaseRepository<GroupProduct> GroupProducts { get; }
        IBaseRepository<Product> Products { get; }
        IBaseRepository<StructureOfTheProduct> StructureOfTheProducts { get; }
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IClientManager ClientManager { get; }




        Task Commit();

        void Rollback();
    }
}