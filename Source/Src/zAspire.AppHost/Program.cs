var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BlazorHostedAspNetCoreApiSingleProject>("blazorhostedaspnetcoreApisingleproject");

builder.Build().Run();
