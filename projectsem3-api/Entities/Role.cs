﻿using System.ComponentModel.DataAnnotations;

namespace projectsem3_api.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
