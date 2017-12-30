using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : IdProvider
    {
        public BaseRepository(IMesContext context)
        {
            Context = context;
        }

        public DbSet<T> Entities => Context.Set<T>();

        public IMesContext Context { get; set; }

        public async Task<IEnumerable<T>> GetAllAsync() => await Entities.ToListAsync();

        public async Task<T> GetAsync(int id) => await Entities.FirstOrDefaultAsync(f => f.Id == id);


        public void Create(T item)
        {
            Entities.Add(item);
        }

        public void Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = Entities.Find(id);
            Context.Entry(entity).State = EntityState.Deleted;
        }
    }
}
