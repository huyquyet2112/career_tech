using CareerTech.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CareerTech.Repo.Interfaces
{
    public class AbstractRepo
    {
        public DatabaseContext DBContext;
        public AbstractRepo(DatabaseContext dbContext)
        {
            DBContext = dbContext;
        }

        public void SaveOne<E>(E entity) where E : class
        {
            var set = DBContext.Set<E>();
            set.Add(entity);
            DBContext.SaveChanges();
        }

        public void SaveMany<E>(IEnumerable<E> entities) where E : class
        {
            foreach (var entity in entities)
            {
                var set = DBContext.Set<E>();
                var entry = DBContext.Entry(entity);

                set.Attach(entity);
                entry.State = EntityState.Modified;
            }
            DBContext.SaveChanges();
        }

        public void UpdateOne<E>(E entity) where E : class
        {
            var set = DBContext.Set<E>();
            UpdateOne(set, entity);
        }

        protected void UpdateOne<E>(DbSet<E> set, E entity) where E : class
        {
            var entry = DBContext.Entry(entity);
            set.Attach(entity);
            entry.State = EntityState.Modified;
            DBContext.SaveChanges();
        }

        public IList<E> GetAll<E>() where E : class
        {
            var set = DBContext.Set<E>();
            return set.ToList();
        }

        public IList<E> FindByParaAndPagging<E, TKey>(Expression<Func<E, bool>> predicate,
                                                      Func<E, TKey> orderBy,
                                                      int pageSize,
                                                      int pageIndex,
                                                      bool desc) where E : class
        {
            var set = DBContext.Set<E>();
            if (desc)
            {
                return set.Where(predicate)
                          .OrderByDescending(orderBy)
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();
            }
            return set.Where(predicate)
                      .OrderBy(orderBy)
                      .Skip((pageIndex - 1) * pageSize)
                      .Take(pageSize).ToList();
        }

        public IList<E> FindByParamAndPagging<E, TKey>(Expression<Func<E, bool>> predicate,
                                                       Func<E, TKey> orderBy,
                                                       int skip,
                                                       int take,
                                                       bool desc) where E : class
        {
            var set = DBContext.Set<E>();
            if (desc)
            {
                return set.Where(predicate)
                          .OrderByDescending(orderBy)
                          .Skip(skip)
                          .Take(take)
                          .ToList();
            }
            return set.Where(predicate)
                      .OrderBy(orderBy)
                      .Skip(skip)
                      .Take(take).ToList();
        }
        public IList<E> FindByParaAndOrder<E, TKey>(Expression<Func<E, bool>> predicate,
                                                      Func<E, TKey> orderBy,
                                                      bool desc) where E : class
        {
            var set = DBContext.Set<E>();
            if (desc)
            {
                return set.Where(predicate)
                          .OrderByDescending(orderBy)
                          .ToList();
            }
            return set.Where(predicate)
                      .OrderBy(orderBy).ToList();
        }

        public E FindOneByParameter<E>(Expression<Func<E, bool>> predicate) where E : class
        {
            var set = DBContext.Set<E>();
            return set.FirstOrDefault(predicate);
        }

        public E FindOneById<E>(long? Id) where E : class
        {
            var set = DBContext.Set<E>();
            return set.Find(Id);
        }

        public IList<E> FindAllByParameter<E>(Expression<Func<E, bool>> predicate) where E : class
        {
            var set = DBContext.Set<E>();
            return set.Where(predicate).ToList();
        }

        public void RemoveOne<E>(E entity) where E : class
        {
            var set = DBContext.Set<E>();
            set.Remove(entity);
            DBContext.SaveChanges();
        }

        public void UpdateMany<E>(Expression<Func<E, bool>> predicate, Action<E> action) where E : class
        {
            var set = DBContext.Set<E>().Where(predicate).ToList();
            set.ForEach(action);
            DBContext.SaveChanges();
        }

        public void RemoveMany<E>(Expression<Func<E, bool>> predicate) where E : class
        {
            var set = DBContext.Set<E>().Where(predicate).ToList();
            set.Clear();
            DBContext.SaveChanges();
        }
    }

    public interface IRepo
    {

    }

    public interface IRepo<E> : IRepo where E : class
    {
        DbSet<E> Entities { get; }
        DatabaseContext DBContext { get; }

        IQueryable<E> GetQueryable();
        void SaveOne(E entity);
        Task SaveOneAsync(E entity);
        void SaveMany(IEnumerable<E> entities);
        Task SaveManyAsync(IEnumerable<E> entities);
        void UpdateOne(E entity);
        Task UpdateOneAsync(E entity);
        IList<E> GetAll();
        Task<IList<E>> GetAllAsync();
        IList<E> FindMany<TKey>(Func<E, TKey> orderBy, bool desc = false);
        IList<E> FindMany<TKey>(int skip, int take, Func<E, TKey> orderBy, bool desc = false);
        IList<E> FindMany(Expression<Func<E, bool>> predicate);
        Task<IList<E>> FindManyAsync(Expression<Func<E, bool>> predicate);
        IList<E> FindMany(Expression<Func<E, bool>> predicate, int skip, int take);
        IList<E> FindMany<TKey>(Expression<Func<E, bool>> predicate,
                                int skip,
                                int take,
                                Func<E, TKey> orderBy,
                                bool desc = false);
        IList<E> FindMany<TKey>(Expression<Func<E, bool>> predicate,
                                Func<E, TKey> orderBy,
                                bool desc = false);
        E FindOne(Expression<Func<E, bool>> predicate);
        Task<E> FindOneAsync(Expression<Func<E, bool>> predicate);
        E FindOneById(object Id);
        Task<E> FindOneByIdAsync(object Id);
        void Remove(IEnumerable<E> entities);
        void Remove(params E[] entities);
        void UpdateMany(IList<E> entities);
        void UpdateMany(Expression<Func<E, bool>> predicate, Action<E> action);
    }

    public abstract class AbstractRepo<E> : IRepo<E> where E : class
    {
        public DbSet<E> Entities { get; protected set; }
        public DatabaseContext DBContext { get; protected set; }

        public virtual IQueryable<E> GetQueryable()
        {
            return Entities.AsQueryable();
        }

        protected AbstractRepo(DatabaseContext dbContext)
        {
            DBContext = dbContext;
            Entities = DBContext.Set<E>();
        }

        public virtual void SaveOne(E entity)
        {
            Entities.Add(entity);
            DBContext.SaveChanges();
        }

        public virtual async Task SaveOneAsync(E entity)
        {
            Entities.Add(entity);
            await DBContext.SaveChangesAsync();
        }

        public virtual void SaveMany(IEnumerable<E> entities)
        {
            Entities.AddRange(entities);
            DBContext.SaveChanges();
        }
        public virtual async Task SaveManyAsync(IEnumerable<E> entities)
        {
            Entities.AddRange(entities);
            await DBContext.SaveChangesAsync();
        }

        public virtual void UpdateOne(E entity)
        {
            DBContext.Update(entity);
            DBContext.SaveChanges();
        }

        public virtual async Task UpdateOneAsync(E entity)
        {
            DBContext.Update(entity);
            await DBContext.SaveChangesAsync();
        }

        public virtual IList<E> GetAll()
        {
            return Entities.ToList();
        }

        public virtual async Task<IList<E>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public virtual IList<E> FindMany<TKey>(Func<E, TKey> orderBy, bool desc = false)
        {
            IQueryable<E> queryBuider = Entities;
            if (desc)
            {
                return queryBuider.OrderByDescending(orderBy).ToList();
            }
            return queryBuider.OrderBy(orderBy).ToList(); ;
        }

        public virtual IList<E> FindMany<TKey>(int skip, int take, Func<E, TKey> orderBy, bool desc = false)
        {
            IQueryable<E> queryBuider = Entities;
            if (desc)
            {
                return queryBuider.OrderByDescending(orderBy).Skip(skip).Take(take).ToList();
            }
            return queryBuider.OrderBy(orderBy).Skip(skip).Take(take).ToList();
        }

        public virtual IList<E> FindMany(Expression<Func<E, bool>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        public virtual async Task<IList<E>> FindManyAsync(Expression<Func<E, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public virtual IList<E> FindMany(Expression<Func<E, bool>> predicate, int skip, int take)
        {
            IQueryable<E> queryBuider = Entities.Where(predicate);
            return queryBuider.Skip(skip).Take(take).ToList();
        }

        public virtual IList<E> FindMany<TKey>(Expression<Func<E, bool>> predicate,
                                                int skip,
                                                int take,
                                                Func<E, TKey> orderBy,
                                                bool desc = false)
        {
            IQueryable<E> queryBuider = Entities.Where(predicate);
            if (desc)
            {
                return queryBuider.OrderByDescending(orderBy).Skip(skip).Take(take).ToList();
            }
            return queryBuider.OrderBy(orderBy).Skip(skip).Take(take).ToList();
        }

        public virtual IList<E> FindMany<TKey>(Expression<Func<E, bool>> predicate,
                                                   Func<E, TKey> orderBy,
                                                   bool desc = false)
        {
            IQueryable<E> queryBuider = Entities.Where(predicate);
            if (desc)
            {
                return queryBuider.OrderByDescending(orderBy).ToList();
            }
            return queryBuider.OrderBy(orderBy).ToList();
        }

        public virtual E FindOne(Expression<Func<E, bool>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public virtual async Task<E> FindOneAsync(Expression<Func<E, bool>> predicate)
        {
            return await Entities.FirstOrDefaultAsync(predicate);
        }

        public virtual E FirstOrDefault()
        {
            return Entities.FirstOrDefault();
        }

        public virtual E FindOneById(object Id)
        {
            var entity = Entities.Find(Id);
            if (entity != null)
                this.DBContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<E> FindOneByIdAsync(object Id)
        {
            var entity = await Entities.FindAsync(Id);
            if (entity != null)
                this.DBContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual void Remove(IEnumerable<E> entities)
        {
            this.Remove(entities.ToArray());
        }

        public virtual void Remove(params E[] entities)
        {
            Entities.RemoveRange(entities);
            DBContext.SaveChanges();
        }

        public virtual void UpdateMany(IList<E> entities)
        {
            Entities.UpdateRange(entities);
            DBContext.SaveChanges();
        }

        public virtual void UpdateMany(Expression<Func<E, bool>> predicate, Action<E> action)
        {
            Entities.Where(predicate).ForEachAsync(action).Wait();
            DBContext.SaveChanges();
        }
    }
}
