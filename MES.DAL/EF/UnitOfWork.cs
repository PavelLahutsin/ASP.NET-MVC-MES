using System;
using System.Linq;
using System.Threading.Tasks;
using MES.DAL.Entities;
using MES.DAL.Identity;
using MES.DAL.Interfaces;
using MES.DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MES.DAL.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly MesContext _context;

        private IBaseRepository<ArrivalOfDetail> _arrivalOfDetailRepository;
        private IBaseRepository<Detail> _detailRepository;
        private IBaseRepository<Product> _productRepository;
        private IBaseRepository<StructureOfTheProduct> _structureOfTheProductRepository;
        private IBaseRepository<GroupProduct> _groupProductRepository;
        private IBaseRepository<DefectDetail> _defectDetailRepository;


        public UnitOfWork()
        {
            _context = new MesContext();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_context));
            ClientManager = new ClientManager(_context);
        }

        public IBaseRepository<ArrivalOfDetail> ArrivalOfDetails => _arrivalOfDetailRepository ??
                (_arrivalOfDetailRepository = new BaseRepository<ArrivalOfDetail>(_context));

        public IBaseRepository<Detail> Details => _detailRepository ??
                                                  (_detailRepository = new BaseRepository<Detail>(_context));

        public IBaseRepository<DefectDetail> DefectDetails => _defectDetailRepository ??
                                                  (_defectDetailRepository = new BaseRepository<DefectDetail>(_context));

        public IBaseRepository<GroupProduct> GroupProducts => _groupProductRepository ??
                                                  (_groupProductRepository = new BaseRepository<GroupProduct>(_context));

        public IBaseRepository<Product> Products => _productRepository ??
                                                    (_productRepository = new BaseRepository<Product>(_context));

        public IBaseRepository<StructureOfTheProduct> StructureOfTheProducts => _structureOfTheProductRepository ??
                               (_structureOfTheProductRepository = new BaseRepository<StructureOfTheProduct>(_context));
        
        public ApplicationUserManager UserManager { get; }

        public IClientManager ClientManager { get; }

        public ApplicationRoleManager RoleManager { get; }


        public async Task Commit()
            => await _context.SaveChangesAsync();

        public void Rollback()
            => _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
                if (disposing)
                    _context.Dispose();

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
