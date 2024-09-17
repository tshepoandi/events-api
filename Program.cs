using System.Collections.Immutable;
using backends.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System;
using Microsoft.AspNetCore.Mvc;
using backends.Entities;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<BackendsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
// app.MapPost("/users", async (BackendsDbContext dbContext, [FromBody] User user) =>
// {
//     dbContext.Users.Add(user);
//     await dbContext.SaveChangesAsync();
//     return Results.Created($"/users/{user.Id}", user);
// });

app.Run();
