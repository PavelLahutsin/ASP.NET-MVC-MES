using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MES.DAL.Entities;

namespace MES.DAL.Interfaces
{
    public interface IMesContext
    {
        DbSet<Detail> Details { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ArrivalOfDetail> ArrivalOfDetails { get; set; }
        DbSet<Soldering> Solderings { get; set; }
        DbSet<DefectDetail> DefectDetails { get; set; }
        DbSet<StructureOfTheProduct> StructureOfTheProducts { get; set; }
        DbSet<ClientProfile> ClientProfiles { get; set; }
        DbSet<GroupProduct> GroupProducts { get; set; }
        DbSet<VariantStateProduct> VariantStateProducts { get; set; }
        DbSet<ProductState> ProductStates { get; set; }
        DbSet<Assembly> Assemblys { get; set; }
        DbSet<CheckJmt> CheckJmts { get; set; }
        DbChangeTracker ChangeTracker { get; }

        int SaveChanges();

        void Dispose();

        DbEntityEntry Entry(object entity);

        DbSet<T> Set<T>() where T : class;
    }
}