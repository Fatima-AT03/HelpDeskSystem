namespace HelpDesk.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public required string Email { get; set; } 

        public required string FullName { get; set; } 

        public int RoleId { get; set; }

        public required string Password { get; set; } 

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
     }
}