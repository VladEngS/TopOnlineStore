global using Microsoft.EntityFrameworkCore;
global using top_online_store_models.Models;
global using Newtonsoft.Json.Linq;
using top_online_store_models;

namespace top_online_store_dblayer
{
    internal class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<StorageProduct> StorageProducts { get; set; }

        private const string configPath = "db_config.json";
        private const string connectionStringKey = "store_ConnectionString";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(configPath))
                throw new Exception($"There is no file for \"{configPath}\" path!");

            var json = JObject.Parse(File.ReadAllText(configPath));

            if (!json.ContainsKey(connectionStringKey))
                throw new Exception($"Inconsistent {connectionStringKey} file!");

            optionsBuilder
                .UseSqlServer((string?)json[connectionStringKey])
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in ModelController.GetModelTypes())
                modelBuilder.Entity(entityType).Property(nameof(IEntity.Id)).HasDefaultValue("NEWSEQUENTIALID()");

            modelBuilder.Entity<Storage>().HasMany(x => x.Products).WithMany(x => x.Storages).UsingEntity(typeof(StorageProduct));
        }
    }
}