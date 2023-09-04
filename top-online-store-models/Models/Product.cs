namespace top_online_store_models.Models
{
    public class Product : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string Name { get; set; } = null!;
        public double Price { get; set; }

        [JsonIgnore] public virtual ICollection<StorageProduct> StorageProducts { get; set; } = null!;
        [JsonIgnore] public virtual ICollection<Storage> Storages { get; set; } = null!;
        [JsonIgnore] public virtual Category Category { get; set; } = null!;
        [JsonIgnore] public virtual Provider Provider { get; set; } = null!;
    }
}
