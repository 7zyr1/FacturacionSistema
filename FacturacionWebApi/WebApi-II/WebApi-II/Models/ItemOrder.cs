using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_II.Models
{
    //[Table("Item_Order")]
    public class ItemOrder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        public required Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int cantidad { get; set; }
    }
}
