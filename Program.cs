using ApiNetCoreAngular.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Add services =====
builder.Services.AddControllers();

// Database: leggere connection string da Environment su Render
builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
            "https://apinetcoreenquiry.netlify.app", // frontend Netlify
            "http://localhost:4200"                   // sviluppo locale
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// ===== Swagger (opzionale) =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== Build app =====
var app = builder.Build();

// ===== Middleware =====

// Applica CORS globale prima di HTTPS e Authorization
app.UseCors("AllowAll");

// Gestione preflight OPTIONS
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});

app.UseHttpsRedirection();
app.UseAuthorization();

// Swagger (opzionale)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enquiry API V1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();

// Porta dinamica per Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");