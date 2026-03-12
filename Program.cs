using ApiNetCoreAngular.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Servizi =====
builder.Services.AddControllers();

// Database: leggi connection string da Environment su Render
builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS globale per frontend Netlify + localhost
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "https://apinetcoreenquiry.netlify.app",
                "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Middleware =====
app.UseRouting();

// Applica CORS globale
app.UseCors();

app.UseAuthorization();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enquiry API V1");
    c.RoutePrefix = "swagger"; // accessibile da /swagger
});

// Controller endpoints
app.MapControllers();

// Root test per verificare se l'app gira
app.MapGet("/", () => "API RUNNING");

// Porta dinamica per Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");