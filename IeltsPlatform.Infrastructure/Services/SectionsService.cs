using IeltsPlatform.Domain.DTOs.Sections;
using IeltsPlatform.Domain.Entities;
using IeltsPlatform.Domain.Enums;
using IeltsPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using Amazon.S3.Model;

namespace IeltsPlatform.Infrastructure.Services;

public interface ISectionsService
{
    Task<Guid> CreateSectionAsync(CreateSectionDto dto);
    Task<Guid> UpdateSectionTranscriptAsync(UpdateSectionDto dto);
    Task<string> UploadSectionAudioAsync(Guid sectionId, Stream audioStream, string fileName);
    Task<Guid> DeleteSectionAsync(DeleteSectionDto dto);
}

public class SectionsService : ISectionsService
{
    private readonly ApplicationDbContext _context;
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public SectionsService(
        ApplicationDbContext context,
        IAmazonS3 s3Client,
        IConfiguration configuration)
    {
        _context = context;
        _s3Client = s3Client;
        _configuration = configuration;
    }

    public async Task<Guid> CreateSectionAsync(CreateSectionDto dto)
    {
        var ieltsTest = await _context.IeltsTests
            .FirstOrDefaultAsync(t => t.Id == dto.TestId && t.DeletedAt == null);

        if (ieltsTest == null)
        {
            throw new InvalidOperationException("IELTS test not found");
        }

        // Only create listening sections initially
        var section = new ListeningSection
        {
            Description = string.Empty,
            Audio = string.Empty,
            Order = (short)dto.Order,
            TestId = ieltsTest.Id
        };

        _context.ListeningSections.Add(section);
        await _context.SaveChangesAsync();

        return section.Id;
    }

    public async Task<Guid> UpdateSectionTranscriptAsync(UpdateSectionDto dto)
    {
        var section = await _context.ListeningSections
            .FirstOrDefaultAsync(s => s.Id == dto.Id && s.DeletedAt == null);

        if (section == null)
        {
            throw new InvalidOperationException("Section not found");
        }

        section.Description = dto.Description;
        await _context.SaveChangesAsync();

        return section.Id;
    }

    public async Task<string> UploadSectionAudioAsync(Guid sectionId, Stream audioStream, string fileName)
    {
        var section = await _context.ListeningSections
            .FirstOrDefaultAsync(s => s.Id == sectionId && s.DeletedAt == null);

        if (section == null)
        {
            throw new InvalidOperationException("Section not found");
        }

        // Upload to S3
        var bucketName = _configuration["AWS:BucketName"];
        var key = $"sections/{Guid.NewGuid()}-{fileName}";

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = audioStream,
            ContentType = "audio/mpeg"
        };

        await _s3Client.PutObjectAsync(request);

        var url = $"https://{bucketName}.s3.amazonaws.com/{key}";
        section.Audio = url;
        await _context.SaveChangesAsync();

        return url;
    }

    public async Task<Guid> DeleteSectionAsync(DeleteSectionDto dto)
    {
        Guid deletedSectionId;
        Guid? testId = null;

        switch (dto.SectionType)
        {
            case SectionCategory.Listening:
                var listeningSection = await _context.ListeningSections
                    .Include(s => s.Test)
                    .FirstOrDefaultAsync(s => s.Id == dto.SectionId);

                if (listeningSection == null)
                {
                    throw new InvalidOperationException($"Listening section with ID {dto.SectionId} not found");
                }

                testId = listeningSection.TestId;
                await DeleteSectionCascadeAsync(dto.SectionId, dto.SectionType);
                _context.ListeningSections.Remove(listeningSection);
                deletedSectionId = listeningSection.Id;
                break;

            case SectionCategory.Reading:
                var readingSection = await _context.ReadingSections
                    .Include(s => s.Test)
                    .FirstOrDefaultAsync(s => s.Id == dto.SectionId);

                if (readingSection == null)
                {
                    throw new InvalidOperationException($"Reading section with ID {dto.SectionId} not found");
                }

                testId = readingSection.TestId;
                await DeleteSectionCascadeAsync(dto.SectionId, dto.SectionType);
                _context.ReadingSections.Remove(readingSection);
                deletedSectionId = readingSection.Id;
                break;

            case SectionCategory.Writing:
                var writingSection = await _context.WritingSections
                    .Include(s => s.Test)
                    .FirstOrDefaultAsync(s => s.Id == dto.SectionId);

                if (writingSection == null)
                {
                    throw new InvalidOperationException($"Writing section with ID {dto.SectionId} not found");
                }

                testId = writingSection.TestId;
                await DeleteSectionCascadeAsync(dto.SectionId, dto.SectionType);
                _context.WritingSections.Remove(writingSection);
                deletedSectionId = writingSection.Id;
                break;

            default:
                throw new InvalidOperationException("Invalid section type");
        }

        await _context.SaveChangesAsync();

        // Reorder sections after deletion
        if (testId.HasValue)
        {
            await ReorderSectionsAsync(testId.Value, dto.SectionType);
        }

        return deletedSectionId;
    }

    private async Task DeleteSectionCascadeAsync(Guid sectionId, SectionCategory sectionType)
    {
        // Get all question groups for this section
        var questionGroups = await _context.QuestionGroups
            .Where(qg => qg.SectionId == sectionId && qg.SectionType == sectionType)
            .ToListAsync();

        foreach (var group in questionGroups)
        {
            // Get all questions in this group
            var questions = await _context.Questions
                .Where(q => q.GroupId == group.Id)
                .ToListAsync();

            foreach (var question in questions)
            {
                // Delete answer keys for this question
                var answerKeys = await _context.AnswerKeys
                    .Where(ak => ak.QuestionId == question.Id)
                    .ToListAsync();

                _context.AnswerKeys.RemoveRange(answerKeys);
            }

            // Delete all questions in this group
            _context.Questions.RemoveRange(questions);
        }

        // Delete all question groups
        _context.QuestionGroups.RemoveRange(questionGroups);
    }

    private async Task ReorderSectionsAsync(Guid testId, SectionCategory sectionType)
    {
        switch (sectionType)
        {
            case SectionCategory.Listening:
                var listeningSections = await _context.ListeningSections
                    .Where(s => s.TestId == testId && s.DeletedAt == null)
                    .OrderBy(s => s.Order)
                    .ToListAsync();

                for (short i = 0; i < listeningSections.Count; i++)
                {
                    short newOrder = (short)(i + 1);
                    if (listeningSections[i].Order != newOrder)
                    {
                        listeningSections[i].Order = newOrder;
                    }

                    await ReorderQuestionGroupsAsync(listeningSections[i].Id, sectionType);
                }
                break;

            case SectionCategory.Reading:
                var readingSections = await _context.ReadingSections
                    .Where(s => s.TestId == testId && s.DeletedAt == null)
                    .OrderBy(s => s.Order)
                    .ToListAsync();

                for (short i = 0; i < readingSections.Count; i++)
                {
                    short newOrder = (short)(i + 1);
                    if (readingSections[i].Order != newOrder)
                    {
                        readingSections[i].Order = newOrder;
                    }

                    await ReorderQuestionGroupsAsync(readingSections[i].Id, sectionType);
                }
                break;

            case SectionCategory.Writing:
                var writingSections = await _context.WritingSections
                    .Where(s => s.TestId == testId && s.DeletedAt == null)
                    .OrderBy(s => s.Order)
                    .ToListAsync();

                for (short i = 0; i < writingSections.Count; i++)
                {
                    short newOrder = (short)(i + 1);
                    if (writingSections[i].Order != newOrder)
                    {
                        writingSections[i].Order = newOrder;
                    }

                    await ReorderQuestionGroupsAsync(writingSections[i].Id, sectionType);
                }
                break;

            default:
                return;
        }

        await _context.SaveChangesAsync();
    }

    private async Task ReorderQuestionGroupsAsync(Guid sectionId, SectionCategory sectionType)
    {
        var groups = await _context.QuestionGroups
            .Where(qg => qg.SectionId == sectionId && qg.SectionType == sectionType && qg.DeletedAt == null)
            .OrderBy(qg => qg.Order)
            .ToListAsync();

        for (short j = 0; j < groups.Count; j++)
        {
            short newOrder = (short)(j + 1);
            if (groups[j].Order != newOrder)
            {
                groups[j].Order = newOrder;
            }

            await ReorderQuestionsAsync(groups[j].Id);
        }
    }

    private async Task ReorderQuestionsAsync(Guid groupId)
    {
        var questions = await _context.Questions
            .Where(q => q.GroupId == groupId && q.DeletedAt == null)
            .OrderBy(q => q.Order)
            .ToListAsync();

        for (short k = 0; k < questions.Count; k++)
        {
            short newOrder = (short)(k + 1);
            if (questions[k].Order != newOrder)
            {
                questions[k].Order = newOrder;
            }
        }
    }
}
