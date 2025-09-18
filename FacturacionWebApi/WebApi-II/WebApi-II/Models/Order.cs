namespace WebApi_II.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime dateTime { get; set; }
        public required string client { get; set; }
        public ICollection<ItemOrder> Items { get; set; }
    }
}
