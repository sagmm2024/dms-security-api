using Andreani.Arq.Observability.Extensions;
using Andreani.Arq.Orleans.Server.Extension;
using Andreani.Arq.WebHost.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAndreaniWebHost(args)
    .AddServerOrleans()
    .AddObservability();

builder.Services
    .ConfigureAndreaniWorkerServices()
    .AddGrainsDependency(builder.Configuration);
builder.Services.AddDefaultCors(builder.Configuration).WithOrigins().Build();

var app = builder.Build();

app.UseCors();
app.UseSecurityHeader(builder.Configuration);
app.ConfigureAndreaniWorker()
    .UseObservability();

app.UseDashboardOrleans();

app.Run();