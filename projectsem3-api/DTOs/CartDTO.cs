using Microsoft.Identity.Client;
using projectsem3_api.Entities;

namespace projectsem3_api.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Quantity { get; set; }
    }
}
