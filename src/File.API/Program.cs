using File.API.Configuration;
using File.API.EndpointBuilders;
using File.API.Extensions;
using File.Core.Configuration;
using File.Infrastructure.Configuration;
using SmallApiToolkit.Middleware;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy => { policy.WithOrigins("http://127.0.0.1:4200", "http://localhost:4200"); });
});

var app = builder.Build();

app.AddCustomContentTypes();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.BuildFileEndpoints();

app.Run();
