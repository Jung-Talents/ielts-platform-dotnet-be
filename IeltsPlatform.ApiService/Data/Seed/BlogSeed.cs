using IeltsPlatform.ApiService.Enums;
using IeltsPlatform.ApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.ApiService.Data.Seed
{
    public static class BlogSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2025, 11, 5, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Blog>().HasData(
                Blog.Create(
                    "IELTS Reading Tips and Strategies",
                    @"Master IELTS Reading with these essential tips:
1. Time management is crucial
2. Skim and scan effectively
3. Practice different question types
4. Build your vocabulary
5. Read academic articles regularly",
                    BlogStatus.Published,
                    BlogTheme.Reading,
                    now,
                    now
                ),
                Blog.Create(
                    "How to Excel in IELTS Speaking Part 2",
                    @"Improve your IELTS Speaking Part 2 performance:
1. Structure your response (past, present, future)
2. Include personal experiences
3. Use advanced vocabulary
4. Practice time management (2 minutes)
5. Record yourself speaking",
                    BlogStatus.Published,
                    BlogTheme.Speaking,
                    now,
                    now
                ),
                Blog.Create(
                    "IELTS Writing Task 2: Essay Structure",
                    @"Perfect your IELTS Writing Task 2 structure:
1. Introduction
   - Background statement
   - Thesis statement
2. Body Paragraphs
   - Topic sentences
   - Supporting points
   - Examples
3. Conclusion
   - Restate main points
   - Final thoughts",
                    BlogStatus.Published,
                    BlogTheme.Writing,
                    now,
                    now
                ),
                Blog.Create(
                    "IELTS Listening: Note-Taking Techniques",
                    @"Enhance your listening skills with these note-taking strategies:
1. Use abbreviations
2. Focus on keywords
3. Practice prediction
4. Listen for signpost words
5. Review and transfer answers carefully",
                    BlogStatus.Published,
                    BlogTheme.Listening,
                    now,
                    now
                )
            );
        }
    }
}
