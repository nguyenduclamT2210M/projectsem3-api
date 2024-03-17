using System.ComponentModel.DataAnnotations;

namespace projectsem3_api.Entities
{
    public class FoodType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
