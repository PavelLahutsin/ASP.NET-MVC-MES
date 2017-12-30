using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using MES.DAL.Entities;

namespace MES.DAL.Interfaces
{
    public interface IBaseRepository<T>
        where T : IdProvider
    {
        Task<IEnumerable<T>> GetAllAsync(); // получение всех объектов
        Task<T> GetAsync(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(int id);
        DbSet<T> Entities { get; }
    }
}