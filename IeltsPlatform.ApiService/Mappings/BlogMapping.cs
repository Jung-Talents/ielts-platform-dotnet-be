using IeltsPlatform.ApiService.Entities;
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
                Name = entity.Name,
                Content = entity.Content,
                Status = entity.Status,
                Theme = entity.Theme,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        public static void ApplyUpdate(this Blog blog, BlogUpdateDto dto)
        {
            if (dto.BlogName != null)
                blog.Name = dto.BlogName;

            if (dto.BlogContent != null)
                blog.Content = dto.BlogContent;

            if (dto.Status.HasValue)
                blog.Status = dto.Status.Value;

            if (dto.Theme.HasValue)
                blog.Theme = dto.Theme.Value;

            blog.UpdatedAt = DateTime.UtcNow;
        }
    }
}
