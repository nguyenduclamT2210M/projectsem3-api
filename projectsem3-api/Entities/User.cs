using System.ComponentModel.DataAnnotations;

namespace projectsem3_api.Entities
{
    public class User
    {
        [Key]
        public int id {  get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string password { get; set; }

    }
}
