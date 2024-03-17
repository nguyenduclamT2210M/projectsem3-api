namespace projectsem3_api.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public RoleDTO Role { get; set; }
    }
}
