using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public string FaIcon { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
