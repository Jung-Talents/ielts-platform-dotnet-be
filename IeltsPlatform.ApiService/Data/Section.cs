using System;
using System.ComponentModel.DataAnnotations;

namespace IeltsPlatform.ApiService.Data
{
    public class Section
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }
    }
}