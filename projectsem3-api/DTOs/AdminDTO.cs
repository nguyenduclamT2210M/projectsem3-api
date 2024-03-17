﻿namespace projectsem3_api.DTOs
{
    public class AdminDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string FullName { get; set; }

        public List<PermissionDTO> Permissions { get; set; }

        public string Token { get; set; }
    }
}
