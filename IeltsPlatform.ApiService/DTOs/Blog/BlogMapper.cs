namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public static class BlogMapper
    {
        public static Entities.Blog CreateCategoryFromDto(CreateBlogRequest dto)
        {
            return Entities.Blog.Create(dto.Blog_name,
                dto.Blog_content,
                dto.Blog_status,
                dto.Blog_theme
            );
        }
    }
}
