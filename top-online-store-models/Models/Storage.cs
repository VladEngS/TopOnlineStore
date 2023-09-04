namespace top_online_store_models.Models
{
    public class Storage : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string Address { get; set; } = null!;
        [NotMapped]public int ProductCount => StorageProducts.Select(x => x.Count).Sum();

        [JsonIgnore] public virtual ICollection<StorageProduct> StorageProducts { get; set; } = null!;
        [JsonIgnore] public virtual ICollection<Product> Products { get; set; } = null!;
    }
}
