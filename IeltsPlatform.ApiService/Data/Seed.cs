using IeltsPlatform.ApiService.Models;

namespace IeltsPlatform.ApiService.Data
{
    public class Seed
    {
        public static void SeedBlogs(AppDbContext context)
        {
            // Check if any blogs already exist
            if (context.Blogs.Any())
            {
                return; // DB has been seeded
            }

            var blogs = new List<Blog>
            {
                new Blog
                {
                    BlogName = "The Power of Consistency in IELTS Preparation",
                    BlogContent = "Many students underestimate the impact of consistent daily practice. In this post, we discuss how short but regular study sessions can help improve IELTS band scores significantly.",
                    Status = Enums.BlogStatus.Published,
                    Theme = Enums.BlogTheme.Reading,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Blog
                {
                    BlogName = "IELTS Writing Task 2: Common Mistakes to Avoid",
                    BlogContent = "Avoiding common grammatical and structural errors can boost your writing score. Learn what examiners expect and how to stay on topic.",
                    Status = Enums.BlogStatus.Published,
                    Theme = Enums.BlogTheme.Writing,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Blog
                {
                    BlogName = "Listening Skills: Why You Should Shadow Native Speakers",
                    BlogContent = "Shadowing helps you internalize rhythm and pronunciation. We explain how to incorporate it into your daily practice.",
                    Status = Enums.BlogStatus.Draft,
                    Theme = Enums.BlogTheme.Listening,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                }
            };

            context.Blogs.AddRange(blogs);
            context.SaveChanges();
        }
    }
}
