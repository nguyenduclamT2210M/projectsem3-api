using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.Entities
{
    
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
