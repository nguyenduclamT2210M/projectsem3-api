using projectsem3_api.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.Models
{
    public class FoodTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
