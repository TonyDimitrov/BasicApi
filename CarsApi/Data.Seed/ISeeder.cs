namespace CarsApi.Data.Seed
{
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext);
    }
}
