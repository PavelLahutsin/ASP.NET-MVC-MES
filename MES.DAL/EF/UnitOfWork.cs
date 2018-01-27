using System;
using System.Linq;
using System.Threading.Tasks;
using MES.DAL.Entities;

using MES.DAL.Interfaces;
using MES.DAL.Repositories;

namespace MES.DAL.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly MesContext _context;

        private IBaseRepository<ArrivalOfDetail> _arrivalOfDetailRepository;
        private IBaseRepository<Detail> _detailRepository;
        private IBaseRepository<Product> _productRepository;
        private IBaseRepository<Soldering> _solderingRepository;
        private IBaseRepository<StructureOfTheProduct> _structureOfTheProductRepository;
        private IBaseRepository<GroupProduct> _groupProductRepository;
        private IBaseRepository<DefectDetail> _defectDetailRepository;
        private IBaseRepository<ProductState> _productStates;
        private IBaseRepository<Assembly> _assembly;
        private IBaseRepository<CheckJmt> _checkJmt;
        private IBaseRepository<Boxing> _boxing;
        private IBaseRepository<Role> _role;
        private IBaseRepository<User> _user;


        public UnitOfWork()
        {
            _context = new MesContext();
           
        }

        public IBaseRepository<ArrivalOfDetail> ArrivalOfDetails => _arrivalOfDetailRepository ??
                (_arrivalOfDetailRepository = new BaseRepository<ArrivalOfDetail>(_context));

        public IBaseRepository<Role> Roles => _role ?? (_role = new BaseRepository<Role>(_context));

        public IBaseRepository<User> Users => _user ?? (_user = new BaseRepository<User>(_context));

        public IBaseRepository<Boxing> Boxings => _boxing ?? (_boxing = new BaseRepository<Boxing>(_context));

        public IBaseRepository<CheckJmt> CheckJmts => _checkJmt ?? (_checkJmt = new BaseRepository<CheckJmt>(_context));

        public IBaseRepository<Assembly> Assemblys => _assembly ??
                                                                    (_assembly = new BaseRepository<Assembly>(_context));

        public IBaseRepository<Detail> Details => _detailRepository ??
                                                  (_detailRepository = new BaseRepository<Detail>(_context));

        public IBaseRepository<DefectDetail> DefectDetails => _defectDetailRepository ??
                                                  (_defectDetailRepository = new BaseRepository<DefectDetail>(_context));

        public IBaseRepository<Soldering> Solderings => _solderingRepository ??
                                                        (_solderingRepository = new BaseRepository<Soldering>(_context));

        public IBaseRepository<GroupProduct> GroupProducts => _groupProductRepository ??
                                                  (_groupProductRepository = new BaseRepository<GroupProduct>(_context));

        public IBaseRepository<Product> Products => _productRepository ??
                                                    (_productRepository = new BaseRepository<Product>(_context));

        public IBaseRepository<StructureOfTheProduct> StructureOfTheProducts => _structureOfTheProductRepository ??
                               (_structureOfTheProductRepository = new BaseRepository<StructureOfTheProduct>(_context));

       

        public IBaseRepository<ProductState> ProductStates => _productStates ??
                                                                                (_productStates = new BaseRepository<ProductState>(_context));
        

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
