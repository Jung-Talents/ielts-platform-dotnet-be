using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using IeltsPlatform.ApiService.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using IeltsPlatform.ApiService.Services.Interfaces;
using IeltsPlatform.ApiService.Services.Implementations;
using IeltsPlatform.ApiService.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

var apiUrl = Environment.GetEnvironmentVariable("ApiBaseUrl") ?? "http://localhost:5381";
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri(apiUrl));

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Configure AWS services
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<Amazon.S3.IAmazonS3>();
builder.Services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// Add services to the container.
builder.Services.AddProblemDetails();

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // convert enums to strings in JSON
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IIeltsTestService, IeltsTestService>();

var app = builder.Build();

// 🔥 Migration logic - chạy khi có environment variable
if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    Console.WriteLine("🔄 Running migrations...");
    await dbContext.Database.MigrateAsync();
    Console.WriteLine("✅ Migrations completed!");
    
    return; // Exit without starting web server
}


// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
