using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public partial class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int FlowerBouquetId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; } = 0;
        [Required]
        public int Quantity { get; set; } = 1;
        [Required]
        public double Discount { get; set; } = 0;

        [JsonIgnore]
        public virtual FlowerBouquet FlowerBouquet { get; set; } = null!;
        [JsonIgnore]
        public virtual Order Order { get; set; } = null!;
    }
}
