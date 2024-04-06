using Infrastructure.Dal.Interfaces;

namespace Infrastrucure.DAL.Repository;

public abstract class BaseRepository<T>:IBaseRepository<T> where T : class
{
    private readonly AplicationContext _context;
    public BaseRepository(AplicationContext aplicationContext)
    {
        _context = aplicationContext;
    }
   
    public void Add(T entity)
    {
        _context.AddAsync(entity);
        _context.SaveChanges();
    }
    public bool Update(T entity)
    {
        
        var findEntity = _context.Set<T>().FirstOrDefault(e => e.Equals(entity));
        if (findEntity != null)
        {
            
            _context.Entry(findEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
    public bool Delete(Guid id)
    {
        var entity= _context.Set<T>().Find(id);
        if (entity==null)
        {
            return false;
        }
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
        return true;
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
}