using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.Entities
{
    public class Combo
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ResId { get; set; }
        [ForeignKey("ResId")]
        public Restaurant Restaurant { get; set; }
    }
}
