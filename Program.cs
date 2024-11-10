using System.Collections.Immutable;
using backends.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Clear known networks since you're behind a Fly.io proxy
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
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
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
        ?? builder.Configuration.GetConnectionString("DefaultConnection");
        
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("No database connection string configured");
    }

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null
        );
    });
    
    options.EnableDetailedErrors();
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
app.UseForwardedHeaders();
// builder.WebHost.UseUrls("http://0.0.0.0:8080;https://0.0.0.0:443");
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Use the new CORS policy
app.UseAuthorization();
app.MapControllers();

app.Run();