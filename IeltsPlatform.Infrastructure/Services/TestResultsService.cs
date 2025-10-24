using IeltsPlatform.Domain.DTOs.TestResults;
using IeltsPlatform.Domain.Entities;
using IeltsPlatform.Domain.Enums;
using IeltsPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IeltsPlatform.Infrastructure.Services;

public interface ITestResultsService
{
    Task<TestResultResponseDto> CreateTestResultAsync(Guid userId, CreateTestResultDto dto);
    Task<TestResultDetailDto> GetTestResultByIdAsync(Guid userId, Guid resultId);
    Task<PaginatedTestResultsDto> GetTestResultsByUserIdAsync(Guid userId, int page, int limit);
}

public class TestResultsService : ITestResultsService
{
    private readonly ApplicationDbContext _context;

    public TestResultsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TestResultResponseDto> CreateTestResultAsync(Guid userId, CreateTestResultDto dto)
    {
        // Load answer keys for the test
        var answerKeys = await _context.AnswerKeys
            .Include(ak => ak.Question)
            .ThenInclude(q => q.Group)
            .Where(ak => ak.TestId == dto.TestId)
            .ToListAsync();

        if (answerKeys.Count == 0)
        {
            throw new InvalidOperationException($"No answer keys found for test ID: {dto.TestId}");
        }

        if (dto.Submission == null || dto.Submission.Count == 0)
        {
            throw new InvalidOperationException("Submission must be a non-empty array");
        }

        // Create answer key map for quick lookup
        var answerKeyMap = answerKeys.ToDictionary(ak => ak.QuestionId, ak => ak);

        // Calculate score
        int correctCount = 0;
        int totalQuestions = 0;

        foreach (var answerKey in answerKeys)
        {
            // Parse the answers JSON to count total questions
            var answersJson = JsonSerializer.Deserialize<List<AnswerGroup>>(answerKey.Answers);
            if (answersJson != null)
            {
                totalQuestions += answersJson.Sum(g => g.Answers?.Count ?? 0);
            }
        }

        // Score each submission
        foreach (var submitted in dto.Submission)
        {
            if (!answerKeyMap.TryGetValue(submitted.QuestionId, out var answerKey))
                continue;

            var expectedAnswers = JsonSerializer.Deserialize<List<AnswerGroup>>(answerKey.Answers);
            if (expectedAnswers == null) continue;

            // Build expected answer map by partIndex
            var expectedPartMap = new Dictionary<int, List<string>>();
            foreach (var group in expectedAnswers)
            {
                if (group.Answers == null) continue;

                foreach (var answer in group.Answers)
                {
                    if (!expectedPartMap.ContainsKey(answer.PartIndex))
                    {
                        expectedPartMap[answer.PartIndex] = new List<string>();
                    }

                    // Add main answer and alternatives (case-insensitive)
                    expectedPartMap[answer.PartIndex].Add(answer.AnswerText.Trim().ToLowerInvariant());
                    
                    if (answer.Alternatives != null)
                    {
                        expectedPartMap[answer.PartIndex].AddRange(
                            answer.Alternatives.Select(alt => alt.Trim().ToLowerInvariant())
                        );
                    }
                }
            }

            // Check user answers
            var questionType = answerKey.Question.Group.Type;

            if (questionType == QuestionType.MultipleChoice || 
                questionType == QuestionType.FillInTheBlank ||
                questionType == QuestionType.TickBox)
            {
                // Check each part
                foreach (var userAnswer in submitted.Answers)
                {
                    if (expectedPartMap.TryGetValue(userAnswer.PartIndex, out var correctAnswers))
                    {
                        var userText = userAnswer.AnswerText.Trim().ToLowerInvariant();
                        if (correctAnswers.Contains(userText))
                        {
                            correctCount++;
                        }
                    }
                }
            }
        }

        // Calculate score (simple percentage for now)
        double score = totalQuestions > 0 ? (double)correctCount / totalQuestions * 100 : 0;

        // Create test result
        var testResult = new IeltsTestResult
        {
            TestId = dto.TestId,
            UserId = userId,
            TotalCorrectAnswers = (short)correctCount,
            Score = (float)score,
            UserSubmission = JsonSerializer.SerializeToDocument(dto.Submission),
            DetailAnalysis = JsonSerializer.SerializeToDocument(new { correctCount, totalQuestions }),
            TimeSpent = 0 // Can be added later
        };

        _context.IeltsTestResults.Add(testResult);
        await _context.SaveChangesAsync();

        return new TestResultResponseDto
        {
            Id = testResult.Id,
            CorrectAnswers = correctCount,
            TotalQuestions = totalQuestions,
            Score = score,
            Details = testResult.DetailAnalysis
        };
    }

    public async Task<TestResultDetailDto> GetTestResultByIdAsync(Guid userId, Guid resultId)
    {
        var result = await _context.IeltsTestResults
            .Where(r => r.Id == resultId && r.UserId == userId)
            .Select(r => new TestResultDetailDto
            {
                Id = r.Id,
                TestId = r.TestId,
                UserId = r.UserId,
                CorrectAnswers = r.TotalCorrectAnswers,
                TotalQuestions = 0, // Will need to calculate from DetailAnalysis
                Score = r.Score,
                Submission = r.UserSubmission,
                Details = r.DetailAnalysis,
                CreatedAt = r.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (result == null)
        {
            throw new InvalidOperationException("Test result not found");
        }

        return result;
    }

    public async Task<PaginatedTestResultsDto> GetTestResultsByUserIdAsync(Guid userId, int page, int limit)
    {
        var query = _context.IeltsTestResults
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt);

        var total = await query.CountAsync();

        var results = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(r => new TestResultDetailDto
            {
                Id = r.Id,
                TestId = r.TestId,
                UserId = r.UserId,
                CorrectAnswers = r.TotalCorrectAnswers,
                TotalQuestions = 0, // Will need to calculate from DetailAnalysis
                Score = r.Score,
                Submission = r.UserSubmission,
                Details = r.DetailAnalysis,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return new PaginatedTestResultsDto
        {
            Data = results,
            Total = total,
            Page = page,
            Limit = limit
        };
    }
}

// Helper classes for JSON deserialization
public class AnswerGroup
{
    public int AnswerGroupId { get; set; }
    public List<Answer>? Answers { get; set; }
}

public class Answer
{
    public int PartIndex { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public List<string>? Alternatives { get; set; }
}
