namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public static class BlogMapper
    {
        public static Entities.Blog CreateCategoryFromDto(CreateBlogRequest dto)
        {
            return Entities.Blog.Create(dto.Name,
                dto.Content,
                dto.Status,
                dto.Theme,
                createdAt: DateTimeOffset.UtcNow,
                updatedAt: DateTimeOffset.UtcNow
            );
        }
    }
}
