using System;
using System.Threading.Tasks;
using MES.DAL.Entities;

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
        IBaseRepository<ProductState> ProductStates { get; }
        IBaseRepository<DefectDetail> DefectDetails { get; }
        IBaseRepository<Assembly> Assemblys { get; }
        IBaseRepository<CheckJmt> CheckJmts { get; }
        IBaseRepository<Role> Roles { get; }
        IBaseRepository<User> Users { get; }
        IBaseRepository<Boxing> Boxings { get; }
        
        Task Commit();

        void Rollback();
    }
}