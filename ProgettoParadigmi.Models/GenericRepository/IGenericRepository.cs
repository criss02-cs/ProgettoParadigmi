using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Estrae dal database per TEntity i record che rispettano filter se presente,
        /// ordinati secondo orderBy se presente, includendo le navigation properties
        /// specificate in includes se presente
        /// </summary>
        /// <param name="filter">L'espressione per cui filtrare</param>
        /// <param name="orderBy">L'ordinamento richiesto</param>
        /// <param name="includes">Eventuali nav prop da includere</param>
        /// <returns></returns>
        List<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes
            );
        /// <summary>
        /// Fornisce un IQueryable per TEntity applicando le condizioni previste da filter
        /// ordinandolo secondo orderBy
        /// </summary>
        /// <param name="filter">L'espressione per cui filtrare</param>
        /// <param name="orderBy">L'ordinamento richiesto</param>
        /// <returns></returns>
        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
            );
        /// <summary>
        /// Torna una sola entity accedendo per primary key
        /// </summary>
        /// <param name="id">Primary key da cercare</param>
        /// <returns></returns>
        TEntity GetById(object id);
        /// <summary>
        /// Torna una sola entity per le condizioni specificate in filter,
        /// includendo le navigation properties specificate in include
        /// </summary>
        /// <param name="filter">L'espressione per cui filtrare</param>
        /// <param name="includes">Eventuali nav prop da includere</param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>>? filter = null,
            params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// Inserisci la entity nel db
        /// </summary>
        /// <param name="entity">La entity da inserire</param>
        void Insert(TEntity entity);
        /// <summary>
        /// Aggiorna la entity nel db
        /// </summary>
        /// <param name="entity">La entity da aggiornare</param>
        void Update(TEntity entity);
        /// <summary>
        /// Elimina la entity di cui ho l'id dal db
        /// </summary>
        /// <param name="id">L'id della entity da eliminare</param>
        void Delete(object id);
        /// <summary>
        /// Esegue l'eliminazione logica della entity di cui ho l'id dal db
        /// </summary>
        /// <param name="id">L'id della entity da eliminare</param>
        void DeleteLogical(TEntity entity);

        bool SaveChanges();
    }
}
