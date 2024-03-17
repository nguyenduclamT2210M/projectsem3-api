using static Azure.Core.HttpHeader;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projectsem3_api.Entities
{
    public class ComboDetail
    {
        [Key]
        public int Id { get; set; }

        public int ComboId { get; set; }
        [ForeignKey("ComboId")]
        public Combo Combo { get; set; }

        public int FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
    }
}
