namespace Infrastructure.Dal.Interfaces
{/// <summary>
    ///Базовый интерфейс CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public interface IBaseRepository<T>
    {
        void Add(T entity);
        bool Update(T entity);
        bool Delete (Guid id);
        IEnumerable<T> GetAll();
    }
}