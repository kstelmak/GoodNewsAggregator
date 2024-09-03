namespace NewsAggregatorApp.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid? RoleId { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; } 

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

    }
}
