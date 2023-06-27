using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public partial class FlowerBouquet
    {
        public FlowerBouquet()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlowerBouquetId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string FlowerBouquetName { get; set; } = null!;
		[Required]
		public string Description { get; set; } = null!;
        [Required]
        [Range(1000, int.MaxValue, ErrorMessage = "Units Price must be greater than 1000.")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Units In Stock must be greater than 0.")]
        public int UnitsInStock { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Status must be 0 or 1")]
        public byte? FlowerBouquetStatus { get; set; }
        [Required]
        public int? SupplierId { get; set; }

        [JsonIgnore]
        public virtual Category? Category { get; set; } = null!;
        [JsonIgnore]
        public virtual Supplier? Supplier { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
