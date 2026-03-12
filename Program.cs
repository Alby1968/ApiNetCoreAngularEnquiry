using ApiNetCoreAngular.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== Add services =====
builder.Services.AddControllers();

// Database: leggere connection string da Environment su Render
builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== CORS =====
// Consentire solo il frontend Netlify
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlify", policy =>
    {
        policy.WithOrigins("https://apinetcoreenquiry.netlify.app") // <--- dominio Netlify corretto
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Enquiry", new OpenApiInfo
    {
        Title = "Enquiry API",
        Version = "V1"
    });
});

var app = builder.Build();

// ===== Middleware =====

// Debug: log degli header per controllare CORS
app.Use(async (context, next) =>
{
    await next();
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        Console.WriteLine($"CORS headers for {context.Request.Path}:");
        foreach (var header in context.Response.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }
    }
});

// **Usa CORS prima di HTTPS e Authorization**
app.UseCors("AllowNetlify");

app.UseHttpsRedirection();

app.UseAuthorization();

// Swagger accessibile in produzione e sviluppo
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/Enquiry/swagger.json", "Enquiry API");
    options.RoutePrefix = "swagger"; // /swagger
});

app.MapControllers();

// ===== Porta dinamica per Render =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");