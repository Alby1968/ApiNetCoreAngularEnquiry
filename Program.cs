using ApiNetCoreAngular.Model;                  // DbContext e modelli
using Microsoft.EntityFrameworkCore;           // EF Core
using Microsoft.OpenApi.Models;                // Swagger

var builder = WebApplication.CreateBuilder(args);

// ===== Servizi =====
builder.Services.AddControllers();

// Database
//builder.Services.AddDbContext<EnquiryDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
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

// ===== PORT FIX PER RENDER =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// ===== Middleware =====
app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enquiry API V1");
    c.RoutePrefix = "swagger";
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", () => "ApiNetCoreAngularEnquiry is running 🚀");


// ===== PORT FIX PER RENDER =====
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

app.Run();