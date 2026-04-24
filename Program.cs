using ApiNetCoreAngular.Model;                  // Il tuo DbContext e modelli
using Microsoft.EntityFrameworkCore;           // EF Core
using Microsoft.OpenApi.Models;                // Swagger
using Swashbuckle.AspNetCore.SwaggerGen;       // Swagger extensions

var builder = WebApplication.CreateBuilder(args);

// ===== Servizi =====
builder.Services.AddControllers();

// Database: leggi connection string da Environment su Render
builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS globale per frontend Netlify + localhost
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.WithOrigins(
//                "https://apinetcoreenquiry.netlify.app",
//                "http://localhost:4200")
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Enquiry API",
        Version = "v1",
        Description = "API per gestione Enquiry"
    });
});

var app = builder.Build();

// ===== Middleware =====
app.UseRouting();

// Applica CORS globale
//app.UseCors();

app.UseAuthorization();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enquiry API V1");
    c.RoutePrefix = "swagger"; // accessibile da /swagger
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.MapControllers();

// Controller endpoints


//app.MapFallbackToFile("index.html");
//// Root test per verificare se l'app gira
//// app.MapGet("/", () => "API RUNNING");

//// Porta dinamica per Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");
app.Run();