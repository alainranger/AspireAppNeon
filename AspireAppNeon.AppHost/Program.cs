using System.Diagnostics;
using AspireAppNeon.AppHost.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = DistributedApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

var connectionString = builder.AddConnectionString(environment);

var apiService = builder.AddProject<Projects.AspireAppNeon_ApiService>("apiservice")
    .WithSwaggerUI()
    .WithScalar()
    .WithRedoc()
    .WithReference(connectionString);

builder.AddProject<Projects.AspireAppNeon_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
