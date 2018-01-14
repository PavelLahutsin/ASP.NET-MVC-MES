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
        IBaseRepository<Soldering> Solderings { get; }
        IBaseRepository<Product> Products { get; }
        IBaseRepository<StructureOfTheProduct> StructureOfTheProducts { get; }
        IBaseRepository<VariantStateProduct> VariantStateProducts { get; }
        IBaseRepository<ProductState> ProductStates { get; }
        IBaseRepository<DefectDetail> DefectDetails { get; }
        IBaseRepository<Assembly> Assemblys { get; }
        IBaseRepository<CheckJmt> CheckJmts { get; }
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IClientManager ClientManager { get; }




        Task Commit();

        void Rollback();
    }
}