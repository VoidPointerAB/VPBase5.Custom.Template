using Microsoft.EntityFrameworkCore;

namespace VPBase.Custom.Core.Data
{
    public interface ICustomDbFactory
    {
        ICustomStorage Create();
    }

    public class CustomDbFactory : ICustomDbFactory
    {
        private readonly DbContextOptions<CustomDatabaseManager> _dbContextOptions;

        public CustomDbFactory(DbContextOptions<CustomDatabaseManager> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public ICustomStorage Create()
        {
            return new CustomDatabaseManager(_dbContextOptions);
        }
    }
}
