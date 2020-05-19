namespace CarsApi.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEntityIRepository<T>
         where T : class
    {
        IQueryable<T> All();

        Task AddAsync(T entity);

        void Edit(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }
}
