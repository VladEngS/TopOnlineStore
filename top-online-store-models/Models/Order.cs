namespace top_online_store_models.Models
{
    public class Order : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [NotMapped] public double TotalPrice => Products.Select(x => x.Price).Sum();

        [JsonIgnore]public virtual ICollection<Product> Products { get; set; } = null!;
        [JsonIgnore] public virtual Client Client { get; set; } = null!;
    }
}
