﻿namespace projectsem3_api.DTOs
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Prefix { get; set; }

        public string FaIcon { get; set; }

        public RoleDTO Role { get; set; }
    }
}
