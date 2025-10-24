using IeltsPlatform.Domain.DTOs.IeltsTests;
using IeltsPlatform.Domain.Entities;
using IeltsPlatform.Domain.Enums;
using IeltsPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IeltsPlatform.Infrastructure.Services;

public interface IIeltsTestService
{
    Task<Guid> CreateTestAsync(CreateIeltsTestDto dto);
    Task<PaginatedIeltsTestsDto> GetTestsBySkillAsync(IeltsTestSkill skill, int page, int limit, string? userRole);
    Task<IeltsTestResponseDto?> GetTestByIdAsync(Guid id);
    Task<IeltsTestResponseDto?> GetTestBySlugAsync(string slug);
    Task<Guid> UpdateTestAsync(Guid id, UpdateIeltsTestDto dto);
    Task<IeltsTest> UpdateTestStatusAsync(Guid id, IeltsTestStatus status);
    Task<Guid> DeleteTestAsync(Guid id);
}

public class IeltsTestService : IIeltsTestService
{
    private readonly ApplicationDbContext _context;

    public IeltsTestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateTestAsync(CreateIeltsTestDto dto)
    {
        // Check if test name already exists
        var existingTest = await _context.IeltsTests
            .FirstOrDefaultAsync(t => t.TestName == dto.TestName);
        
        if (existingTest != null)
        {
            throw new InvalidOperationException("Test name already exists");
        }

        var slug = await GenerateSlugAsync(dto.TestName);

        var test = new IeltsTest
        {
            TestName = dto.TestName,
            Skill = dto.Skill,
            Duration = dto.Duration,
            Order = dto.Order,
            Slug = slug,
            Status = IeltsTestStatus.Draft
        };

        _context.IeltsTests.Add(test);
        await _context.SaveChangesAsync();

        return test.Id;
    }

    public async Task<PaginatedIeltsTestsDto> GetTestsBySkillAsync(
        IeltsTestSkill skill, 
        int page, 
        int limit, 
        string? userRole)
    {
        var query = _context.IeltsTests.Where(t => t.Skill == skill && t.DeletedAt == null);

        // Filter by published status for students
        if (userRole == UserRole.Student.ToString() || string.IsNullOrEmpty(userRole))
        {
            query = query.Where(t => t.Status == IeltsTestStatus.Published);
        }

        var totalItems = await query.CountAsync();
        var tests = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(t => new IeltsTestResponseDto
            {
                Id = t.Id,
                TestName = t.TestName,
                Skill = t.Skill,
                Duration = t.Duration,
                Order = t.Order,
                Status = t.Status,
                Slug = t.Slug,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return new PaginatedIeltsTestsDto
        {
            Data = tests,
            Meta = new PaginationMeta
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)limit),
                TotalItems = totalItems,
                ItemsPerPage = limit
            }
        };
    }

    public async Task<IeltsTestResponseDto?> GetTestByIdAsync(Guid id)
    {
        var test = await _context.IeltsTests
            .Where(t => t.Id == id && t.DeletedAt == null)
            .Select(t => new IeltsTestResponseDto
            {
                Id = t.Id,
                TestName = t.TestName,
                Skill = t.Skill,
                Duration = t.Duration,
                Order = t.Order,
                Status = t.Status,
                Slug = t.Slug,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        return test;
    }

    public async Task<IeltsTestResponseDto?> GetTestBySlugAsync(string slug)
    {
        var test = await _context.IeltsTests
            .Where(t => t.Slug == slug && t.DeletedAt == null && t.Status == IeltsTestStatus.Published)
            .Select(t => new IeltsTestResponseDto
            {
                Id = t.Id,
                TestName = t.TestName,
                Skill = t.Skill,
                Duration = t.Duration,
                Order = t.Order,
                Status = t.Status,
                Slug = t.Slug,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        return test;
    }

    public async Task<Guid> UpdateTestAsync(Guid id, UpdateIeltsTestDto dto)
    {
        var test = await _context.IeltsTests.FindAsync(id);
        if (test == null)
        {
            throw new InvalidOperationException("Test not found");
        }

        // Check if new name conflicts with another test
        if (dto.TestName != test.TestName)
        {
            var existingTest = await _context.IeltsTests
                .FirstOrDefaultAsync(t => t.TestName == dto.TestName && t.Id != id);
            
            if (existingTest != null)
            {
                throw new InvalidOperationException("Test name already exists");
            }

            test.TestName = dto.TestName;
            test.Slug = await GenerateSlugAsync(dto.TestName);
        }

        test.Duration = dto.Duration;
        await _context.SaveChangesAsync();

        return test.Id;
    }

    public async Task<IeltsTest> UpdateTestStatusAsync(Guid id, IeltsTestStatus status)
    {
        var test = await _context.IeltsTests.FindAsync(id);
        if (test == null)
        {
            throw new InvalidOperationException("Test not found");
        }

        test.Status = status;
        await _context.SaveChangesAsync();

        return test;
    }

    public async Task<Guid> DeleteTestAsync(Guid id)
    {
        var test = await _context.IeltsTests.FindAsync(id);
        if (test == null)
        {
            throw new InvalidOperationException("Test not found");
        }

        // Soft delete
        test.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return test.Id;
    }

    private async Task<string> GenerateSlugAsync(string testName)
    {
        var baseSlug = Slugify(testName);
        var slug = baseSlug;
        var counter = 0;

        while (true)
        {
            var existing = await _context.IeltsTests
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Slug == slug);

            if (existing == null) break;

            var shortId = Guid.NewGuid().ToString("N")[..6];
            slug = $"{baseSlug}-{shortId}";

            counter++;
            if (counter > 5)
            {
                throw new InvalidOperationException("Cannot generate unique slug");
            }
        }

        return slug;
    }

    private static string Slugify(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-");
    }
}
