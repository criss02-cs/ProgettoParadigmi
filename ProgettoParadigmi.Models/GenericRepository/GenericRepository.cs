using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProgettoParadigmi.Models.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppuntamentiDbContext _context;
        private readonly DbSet<T> _table;

        public GenericRepository(AppuntamentiDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }


        public List<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _table;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList(); ;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _table;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        public T GetById(object id)
        {
            return _table.Find(id);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _table;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            //si, rischio di trovarlo null ma se ho scritto un FirstOrDefault senza condizione
            //mi merito l'errore :D
            return query.FirstOrDefault(filter);
        }

        public void Insert(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _table.Add(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _table.Attach(entity);
            //_context.DetachLocal(entity, entity.Id);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T entityToDelete = _table.Find(id);
            _table.Remove(entityToDelete);
        }

        public void DeleteLogical(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.IsDeleted = true;
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public bool SaveChanges() => _context.SaveChanges() > 0;
    }
}
