using System;

namespace IeltsPlatform.ApiService.Dtos
{
    public class CreateSectionDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}