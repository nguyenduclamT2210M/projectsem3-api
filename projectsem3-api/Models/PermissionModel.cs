using System.ComponentModel.DataAnnotations;

namespace projectsem3_api.Models
{
    public class PermissionModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Prefix is required.")]
        public string Prefix { get; set; }
        [Required(ErrorMessage = "FaIcon is required.")]
        public string FaIcon { get; set; }
        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
    }
}
