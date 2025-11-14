using IeltsPlatform.ApiService.Models;
using IeltsPlatform.ApiService.DTOs;
using IeltsPlatform.ApiService.DTOs.Blog;

namespace IeltsPlatform.ApiService.Mappings
{
    public static class BlogMapping
    {
        public static BlogResponseDto ToResponseDto(this Blog entity)
        {
            return new BlogResponseDto
            {
                Id = entity.Id,
                BlogName = entity.BlogName,
                BlogContent = entity.BlogContent,
                Status = entity.Status,
                Theme = entity.Theme,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        public static void ApplyUpdate(this Blog blog, BlogUpdateDto dto)
        {
            if (dto.BlogName != null)
                blog.BlogName = dto.BlogName;

            if (dto.BlogContent != null)
                blog.BlogContent = dto.BlogContent;

            if (dto.Status.HasValue)
                blog.Status = dto.Status.Value;

            if (dto.Theme.HasValue)
                blog.Theme = dto.Theme.Value;

            blog.UpdatedAt = DateTime.UtcNow;
        }

        public static Blog ToEntity(this BlogRequestDto dto)
        {
            return new Blog
            {
                Id = Guid.NewGuid(),
                BlogName = dto.BlogName,
                BlogContent = dto.BlogContent,
                Status = dto.Status,
                Theme = dto.Theme,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = DateTime.MinValue
            };
        }
    }
}
