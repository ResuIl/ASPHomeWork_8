namespace ASPHomeWork_8.Models;

public class Tag
{
    public string Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedTime { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
