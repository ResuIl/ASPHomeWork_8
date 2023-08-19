using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ASPHomeWork_8.Models;

public class Product
{
    private static int _id = 0;

    public string Id { get; set; }

    [DisplayName("Product Name")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "It is required")]
    public string? Name { get; set; }

    public string Description { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "It is required")]
    [RegularExpression(@"^\$?\d+(\.(\d{1}))?$")]
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string CategoryId { get; set; }

    public DateTime CreatedTime { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
