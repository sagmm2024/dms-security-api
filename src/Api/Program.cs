using Andreani.Arq.Observability.Extensions;
using Andreani.Arq.Orleans.Client.Extension;
using Andreani.Arq.WebHost.Extension;
using SecurityApi.Application;
using SecurityApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAndreaniWebHost(args)
    .ConfigureOrleansClient()
    .AddObservability();
builder.Services.ConfigureAndreaniServices()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddDefaultCors(builder.Configuration).WithOrigins().Build();

var app = builder.Build();

app.UseCors();
app.UseSecurityHeader(builder.Configuration);
app.ConfigureAndreani().UseObservability();

await Task.Delay(10000);

app.Run();