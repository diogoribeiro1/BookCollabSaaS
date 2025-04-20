using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Infrastructure.Data;
using BookCollabSaaS.Infrastructure.Repositories;
using BookCollabSaaS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("DataSource=:memory:")
);
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.OpenConnection();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseWebSockets();
app.MapControllers();

app.Run();