var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.IeltsPlatform_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.IeltsPlatform_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
