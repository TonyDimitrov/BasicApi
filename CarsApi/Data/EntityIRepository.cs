namespace CarsApi.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public class EntityIRepository<T> : IEntityIRepository<T>
        where T : class
    {
        public EntityIRepository(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.Entities = dbContext.Set<T>();
        }

        public ApplicationDbContext DbContext { get; set; }

        public DbSet<T> Entities { get; set; }

        public virtual IQueryable<T> All()
        {
            return this.Entities;
        }

        public virtual async Task AddAsync(T entity)
        {
            await this.Entities.AddAsync(entity);
        }

        public virtual void Edit(T entity)
        {
            this.Entities.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            this.Entities.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
           return await this.DbContext.SaveChangesAsync();
        }
    }
}
