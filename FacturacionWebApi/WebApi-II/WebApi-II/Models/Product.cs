using System.ComponentModel.DataAnnotations;

namespace WebApi_II.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
    }
}
