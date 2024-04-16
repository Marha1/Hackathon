    using Domain.Enities;

    namespace Infrastructure.DAL.Interfaces
    {/// <summary>
        ///Базовый интерфейс CRUD
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IBaseRepository<T> where T : BaseEntity
        {
            Task Add(T entity);
            Task<bool> Update(T entity);
            Task<bool> Delete (Guid id);
            Task<IEnumerable<T>> GetAll();
        }
    }