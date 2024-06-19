using File.API.Configuration;
using File.API.EndpointBuilders;
using File.API.Extensions;
using File.Core.Configuration;
using File.Infrastructure.Configuration;
using SmallApiToolkit.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddCustomContentTypes();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.BuildFileEndpoints();

app.Run();
