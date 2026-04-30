using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVIZI =====
builder.Services.AddControllers();

// HttpClient (necessario per Supabase REST)
builder.Services.AddHttpClient();

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("https://effervescent-haupia-0f73ab.netlify.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ===== SWAGGER =====
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

// ===== MIDDLEWARE =====
app.UseRouting();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enquiry API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", () => "ApiNetCoreAngularEnquiry is running 🚀");

// ===== RENDER PORT FIX =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
