using IeltsPlatform.Domain.DTOs.QuestionGroups;
using IeltsPlatform.Domain.Entities;
using IeltsPlatform.Domain.Enums;
using IeltsPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using Amazon.S3.Model;

namespace IeltsPlatform.Infrastructure.Services;

public interface IQuestionGroupsService
{
    Task<Guid> CreateQuestionGroupAsync(CreateQuestionGroupDto dto);
    Task<Guid> UpdateQuestionGroupAsync(UpdateQuestionGroupDto dto);
    Task<string> UploadQuestionImageAsync(Guid groupId, Stream imageStream, string fileName);
    Task<string?> RemoveQuestionImageAsync(Guid groupId);
    Task<Guid> DeleteQuestionGroupAsync(Guid groupId);
}

public class QuestionGroupsService : IQuestionGroupsService
{
    private readonly ApplicationDbContext _context;
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public QuestionGroupsService(
        ApplicationDbContext context,
        IAmazonS3 s3Client,
        IConfiguration configuration)
    {
        _context = context;
        _s3Client = s3Client;
        _configuration = configuration;
    }

    public async Task<Guid> CreateQuestionGroupAsync(CreateQuestionGroupDto dto)
    {
        // Validate section exists
        var sectionExists = dto.SectionType switch
        {
            SectionCategory.Listening => await _context.ListeningSections
                .AnyAsync(s => s.Id == dto.SectionId && s.DeletedAt == null),
            SectionCategory.Reading => await _context.ReadingSections
                .AnyAsync(s => s.Id == dto.SectionId && s.DeletedAt == null),
            SectionCategory.Writing => await _context.WritingSections
                .AnyAsync(s => s.Id == dto.SectionId && s.DeletedAt == null),
            _ => false
        };

        if (!sectionExists)
        {
            throw new InvalidOperationException($"{dto.SectionType} section not found");
        }

        var group = new QuestionGroup
        {
            SectionId = dto.SectionId,
            SectionType = dto.SectionType,
            Instruction = string.Empty,
            Type = dto.Type,
            Order = (short)dto.Order
        };

        _context.QuestionGroups.Add(group);
        await _context.SaveChangesAsync();

        return group.Id;
    }

    public async Task<Guid> UpdateQuestionGroupAsync(UpdateQuestionGroupDto dto)
    {
        var existingGroup = await _context.QuestionGroups
            .FirstOrDefaultAsync(qg => qg.Id == dto.Id && qg.DeletedAt == null);

        if (existingGroup == null)
        {
            throw new InvalidOperationException("Question group not found");
        }

        existingGroup.Instruction = dto.Instruction;
        existingGroup.Type = dto.Type;
        existingGroup.Order = (short)dto.Order;
        existingGroup.Category = dto.Category;
        existingGroup.Content = dto.Content;

        await _context.SaveChangesAsync();

        return existingGroup.Id;
    }

    public async Task<string> UploadQuestionImageAsync(Guid groupId, Stream imageStream, string fileName)
    {
        var questionGroup = await _context.QuestionGroups
            .FirstOrDefaultAsync(qg => qg.Id == groupId && qg.DeletedAt == null);

        if (questionGroup == null)
        {
            throw new InvalidOperationException($"Question group with ID {groupId} not found");
        }

        // Upload to S3
        var bucketName = _configuration["AWS:BucketName"];
        var key = $"question-groups/{Guid.NewGuid()}-{fileName}";

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = imageStream,
            ContentType = GetContentType(fileName)
        };

        await _s3Client.PutObjectAsync(request);

        var url = $"https://{bucketName}.s3.amazonaws.com/{key}";
        questionGroup.Image = url;
        await _context.SaveChangesAsync();

        return url;
    }

    public async Task<string?> RemoveQuestionImageAsync(Guid groupId)
    {
        var questionGroup = await _context.QuestionGroups
            .FirstOrDefaultAsync(qg => qg.Id == groupId && qg.DeletedAt == null);

        if (questionGroup == null)
        {
            throw new InvalidOperationException($"Question group with ID {groupId} not found");
        }

        questionGroup.Image = null;
        await _context.SaveChangesAsync();

        return null;
    }

    public async Task<Guid> DeleteQuestionGroupAsync(Guid groupId)
    {
        var questionGroup = await _context.QuestionGroups
            .FirstOrDefaultAsync(qg => qg.Id == groupId && qg.DeletedAt == null);

        if (questionGroup == null)
        {
            throw new InvalidOperationException("Question group not found");
        }

        var sectionId = questionGroup.SectionId;
        var sectionType = questionGroup.SectionType;

        // Get all questions in this group
        var questions = await _context.Questions
            .Where(q => q.GroupId == groupId)
            .ToListAsync();

        // Delete answer keys for all questions
        foreach (var question in questions)
        {
            var answerKeys = await _context.AnswerKeys
                .Where(ak => ak.QuestionId == question.Id)
                .ToListAsync();
            _context.AnswerKeys.RemoveRange(answerKeys);
        }

        // Delete all questions
        _context.Questions.RemoveRange(questions);

        // Delete the question group
        _context.QuestionGroups.Remove(questionGroup);
        await _context.SaveChangesAsync();

        // Reorder remaining groups in the same section
        var remainingGroups = await _context.QuestionGroups
            .Where(qg => qg.SectionId == sectionId && 
                         qg.SectionType == sectionType && 
                         qg.DeletedAt == null)
            .OrderBy(qg => qg.Order)
            .ToListAsync();

        for (short i = 0; i < remainingGroups.Count; i++)
        {
            short newOrder = (short)(i + 1);
            if (remainingGroups[i].Order != newOrder)
            {
                remainingGroups[i].Order = newOrder;
            }
        }

        await _context.SaveChangesAsync();

        return groupId;
    }

    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}
