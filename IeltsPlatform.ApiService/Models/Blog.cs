using IeltsPlatform.ApiService.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IeltsPlatform.ApiService.Models
{
    public class Blog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string BlogName { get; set; } = string.Empty;

        [Required]
        public string BlogContent { get; set; } = string.Empty;

        [Required]
        public BlogStatus Status { get; set; } = BlogStatus.Draft;

        [Required]
        public BlogTheme Theme { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? DeletedAt { get; set; }
    }
}
