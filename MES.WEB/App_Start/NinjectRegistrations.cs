using MES.BLL.Interfaces;
using MES.BLL.Services;
using MES.DAL.EF;
using MES.DAL.Interfaces;
using MES.DAL.Repositories;
using Ninject.Modules;

namespace MES.WEB
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IMesContext>().To<MesContext>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IStockService>().To<StockService>();
            Bind(typeof(IBaseRepository<>)).To(typeof(BaseRepository<>));
            Bind<ISolderingService>().To<SolderingService>();
            Bind<IArrivalService>().To<ArrivalService>();
            Bind<IAssemblyService>().To<AssemblyService>();
            Bind<ICheckJmtService>().To<CheckJmtService>();
            Bind<IBoxingService>().To<BoxingService>();
            Bind<IFinishedGoodsWarehouseService>().To<FinishedGoodsWarehouseServiceService>();
        }
    }
}
