namespace top_online_store_models.Models
{
    public class Client : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string Name { get; set; } = null!;
        [Required] public string Surname { get; set; } = null!;
        [JsonIgnore][Required] public string Login { get; set; } = null!;
        [JsonIgnore] public string Password { get; set; } = null!;

        [JsonIgnore] public virtual ICollection<Order> Orders { get; set; } = null!;
    }
}
