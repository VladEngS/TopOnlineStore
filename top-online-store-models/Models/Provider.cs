namespace top_online_store_models.Models
{
    public class Provider : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string Name { get; set; } = null!;
        [JsonIgnore][Required] public string Login { get; set; } = null!;
        [JsonIgnore] public string Password { get; set; } = null!;

        [JsonIgnore] public virtual ICollection<Product> Products { get; set; } = null!;
    }
}
