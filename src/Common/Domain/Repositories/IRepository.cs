using System.Linq;

namespace Domain.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        T GetById(int id);

        void Save(T entity);

        void RemoveById(int id);

        void Update(int id, T entity);
    }
}
