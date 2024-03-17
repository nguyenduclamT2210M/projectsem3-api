using projectsem3_api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User UserDTO { get; set; }

        public int Quantity { get; set; }
    }
}
