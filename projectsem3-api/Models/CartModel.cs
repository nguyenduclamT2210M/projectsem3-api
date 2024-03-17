using projectsem3_api.DTOs;
using projectsem3_api.Entities;

namespace projectsem3_api.Models
{
    public class CartModel
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public UserDTO User { get; set; }
        public int Quantity { get; set; }
    }
}
