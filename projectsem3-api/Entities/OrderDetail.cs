using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
    }
}
