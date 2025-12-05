using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.Entities
{
    public sealed class User
    {
        private User() { }
        
        private User(
            string avatarLink,
            string email,
            string username,
            string phoneNumber,
            string password,
            UserRole role,
            DateTimeOffset createdAt,
            DateTimeOffset? updatedAt)
        {
            Id = Guid.NewGuid();
            AvatarLink = avatarLink;
            Email = email;
            Username = username;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = role;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static User Create(
            string avatarLink,
            string email,
            string username,
            string phoneNumber,
            string password,
            UserRole role,
            DateTimeOffset createdAt,
            DateTimeOffset? updatedAt)
        {
            return new User(avatarLink, email, username, phoneNumber, password, role, createdAt, updatedAt);
        }

        public Guid Id { get; set; }
        public string AvatarLink { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }
    }
}

