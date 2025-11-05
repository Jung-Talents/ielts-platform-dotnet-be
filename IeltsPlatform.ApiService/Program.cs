using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using IeltsPlatform.ApiService.Properties.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//SQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

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

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // convert enums to strings in JSON
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); ;

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
