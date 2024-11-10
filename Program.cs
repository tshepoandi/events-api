using System.Collections.Immutable;
using backends.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Events API",
        Version = "v1",
        Description = "Api for an events database",
        Contact = new OpenApiContact
        {
            Name = "Tshepo Samuel Mashiloane",
            Email = "tshepomashiloane869@gmail.com",
            Url = new Uri("https://github.com/tshepoandi")
        }
    });
    
    // Include XML comments if you have them
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Configure CORS to allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Allow any origin
                   .AllowAnyMethod() // Allow any method (GET, POST, etc.)
                   .AllowAnyHeader(); // Allow any header
        });
});

// Configure Database
builder.Services.AddDbContext<BackendsDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => {
            x.EnableRetryOnFailure(
                maxRetryCount: 5, 
                maxRetryDelay: TimeSpan.FromSeconds(30), 
                errorCodesToAdd: null
            );
        }
    );
    
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging(); // Use carefully in production
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty; // Serves the Swagger UI at the root URL
    });
}
app.Urls.Add("http://0.0.0.0:80");
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Use the new CORS policy
app.UseAuthorization();
app.MapControllers();

app.Run();