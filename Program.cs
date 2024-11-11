using System.Collections.Immutable;
using backends.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Configure forwarded headers early in the pipeline
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | 
                              ForwardedHeaders.XForwardedProto |
                              ForwardedHeaders.XForwardedHost;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger only for development
if (builder.Environment.IsDevelopment())
{
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
        
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    });
}

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
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
    
    // Only enable detailed errors in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
    }
});

var app = builder.Build();

// Apply forwarded headers early in the pipeline
app.UseForwardedHeaders();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty;
    });
}

// Ensure HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();

// Configure the listening URL
app.Urls.Add("http://0.0.0.0:8080");

app.Run();