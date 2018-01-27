using System.Data.Entity;
using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.DAL.EF
{
    public class MesContext : DbContext, IMesContext
    {
        public DbSet<Detail> Details { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ArrivalOfDetail> ArrivalOfDetails { get; set; }
        public DbSet<Soldering> Solderings { get; set; }
        public DbSet<DefectDetail> DefectDetails { get; set; }
        public DbSet<StructureOfTheProduct> StructureOfTheProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<GroupProduct> GroupProducts { get; set; }
        public DbSet<ProductState> ProductStates { get; set; }
        public DbSet<Assembly> Assemblys { get; set; }
        public DbSet<CheckJmt> CheckJmts { get; set; }
        public DbSet<Boxing> Boxings { get; set; }


        static MesContext()
        {
            Database.SetInitializer<MesContext>(new MesDbInitializer());
        }
        public MesContext()
            : base("MesContextDb")
        {
        }
    }
}
