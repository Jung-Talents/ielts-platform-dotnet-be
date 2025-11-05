namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public static class BlogMapper
    {
        public static Entities.Blog CreateCategoryFromDto(CreateBlogRequest dto)
        {
            return Entities.Blog.Create(dto.BlogName,
                dto.BlogContent,
                dto.BlogStatus,
                dto.BlogTheme
            );
        }
    }
}
