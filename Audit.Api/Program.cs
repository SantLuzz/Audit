using Audit.Domain.Repositories;
using Audit.Infra.Data;
using Audit.Infra.Repositories;
using Audit.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Configuration.ConnectionString = builder.Configuration
    .GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x
     => x.UseSqlServer(Configuration.ConnectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger();
}

app.Run();
