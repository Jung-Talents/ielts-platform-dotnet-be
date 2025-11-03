using System;
using IeltsPlatform.ApiService.Enums;
using System.ComponentModel.DataAnnotations;
using OpenTelemetry.Trace;

namespace IeltsPlatform.ApiService.Entities
{
    public class Blog
    {
        [Key] //primary key
        //id: (GUID) - The unique identifier for the Blog post.
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(256)]
        //blog_name: (string) - The title of the blog post.
        public string BlogName { get; set; } = string.Empty;

        //blog_content: (string) - The main body content of the blog post.
        public string BlogContent { get; set; } = string.Empty;

        //status: (enum) - The current status of the blog post (e.g., 'draft', 'published').
        public BlogStatus Status { get; set; }

        //theme: (enum) - The theme or category of the blog post (e.g., 'Listening', 'Reading', 'Speaking', 'Writing').
        public BlogTheme Theme { get; set; }

        //created_at: (timestamp) - The timestamp when the blog post was created.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //updated_at: (timestamp) - The timestamp when the blog post was last updated.
        public DateTime UpdatedAt { get; set; }

        //deleted_at: (timestamp) - The timestamp when the blog post was soft-deleted.
        public DateTime DeletedAt { get; set; }


    }
}