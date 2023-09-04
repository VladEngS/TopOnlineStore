namespace top_online_store_models.Models
{
    public class StorageProduct : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        public int Count { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Storage Storage { get; set; } = null!;
    }
}
