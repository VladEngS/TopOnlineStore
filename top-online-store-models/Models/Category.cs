namespace top_online_store_models.Models
{
    public class Category : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string Name { get; set; } = null!;

        [JsonIgnore] public virtual ICollection<Product> Products { get; set; } = null!;
    }
}
