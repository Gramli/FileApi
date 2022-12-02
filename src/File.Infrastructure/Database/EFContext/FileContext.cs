using File.Infrastructure.Database.EFContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace File.Infrastructure.Database.EFContext
{
    internal class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options)
            : base(options) { }

        public virtual DbSet<FileEntity> Files => Set<FileEntity>();
    }
}
