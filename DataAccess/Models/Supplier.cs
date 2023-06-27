using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            FlowerBouquets = new HashSet<FlowerBouquet>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        public string? SupplierName { get; set; }

		[Required]
		public string? SupplierAddress { get; set; }

        [Required]
        public string? Telephone { get; set; }

        [JsonIgnore]
        public virtual ICollection<FlowerBouquet> FlowerBouquets { get; set; }
    }
}
