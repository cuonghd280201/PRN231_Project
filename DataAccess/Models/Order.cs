using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? ShippedDate { get; set; }
        [Required]
        public decimal? Total { get; set; } = 0;
        [Required]
        public string? OrderStatus { get; set; } = "Waiting";
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
