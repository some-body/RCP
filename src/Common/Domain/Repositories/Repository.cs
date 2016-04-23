using Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Domain.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected static DbContext _dbContext;

        static Repository()
        {
        }

        public IQueryable<T> GetAll()
        {
            return GetEntityList();
        }

        public T GetById(int id)
        {
            var result = GetEntityList().FirstOrDefault(e => e.Id == id);
            if (result == null)
                throw new EntityNotFoundException();

            return result;
        }

        public virtual void Save(T entity)
        {
            var list = GetEntityList();
            if (entity.Id.HasValue && list.Any(e => e.Id == entity.Id.Value))
            {
                System.IO.File.AppendAllText("C:/St/111.txt", "1\r\n");
                Update(entity.Id.Value, entity);
                System.IO.File.AppendAllText("C:/St/111.txt", "2\r\n");
            }
            else
            {
                list.Add(entity);
            }
            System.IO.File.AppendAllText("C:/St/111.txt", "тут я был\r\n");
            _dbContext.SaveChanges();
            System.IO.File.AppendAllText("C:/St/111.txt", "тут 2 был\r\n");
        }

        public void Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityList = GetEntityList();
            entityList.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void RemoveById(int id)
        {
            var entityList = GetEntityList();
            var entity = entityList.FirstOrDefault(e => e.Id == id);
            if (entity == null)
                throw new EntityNotFoundException();

            Remove(entity);
        }

        public void Patch(int id, T entity)
        {
            entity.Id = null;
            var existingEntity = GetEntityList().Find(id);
            if (existingEntity == null)
                throw new EntityNotFoundException();

            PatchEntityValues(entity, existingEntity);
            _dbContext.SaveChanges();
        }

        public virtual void Update(int id, T entity)
        {
            System.IO.File.AppendAllText("C:/St/111.txt", "Update\r\n");
            entity.Id = null;

            var existingEntity = GetEntityList().Find(id);
            if (existingEntity == null)
                throw new EntityNotFoundException();

            UpdateEntityValues(entity, existingEntity);
            //_dbContext.SaveChanges();
        }

        private void PatchEntityValues(T source, T target)
        {
            var props = typeof(T).GetProperties();
            foreach (var property in props)
            {
                var value = property.GetValue(source);
                if (value != null)
                    property.SetValue(target, value);
            }
        }

        private void UpdateEntityValues(T source, T target)
        {
            var props = typeof(T).GetProperties();
            foreach (var property in props)
            {
                if (property.Name == "Id")
                    continue;

                var value = property.GetValue(source);
                if(value != null)
                    property.SetValue(target, value);
            }
        }

        protected abstract DbSet<T> GetEntityList();

        /*
        public static Repository<TEntity> For<TEntity>() where TEntity : Entity
        {
            return new Repository<TEntity>();
        }
        */
    }
}
