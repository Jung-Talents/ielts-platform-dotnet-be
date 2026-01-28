using System.ComponentModel.DataAnnotations;

namespace IeltsPlatform.ApiService.Data
{
    public enum BlogStatus
    {
        Draft,
        Published,
        Archived
    }

    public enum BlogTheme
    {
        Reading,
        Speaking,
        Writing,
        Listening
    }

    public class Blog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string BlogName { get; set; } = string.Empty;

        public string BlogContent { get; set; } = string.Empty;

        public BlogStatus Status { get; set; }

        public BlogTheme Theme { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}