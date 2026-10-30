using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// SERVIZI
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// CORS: consenti solo il frontend Netlify
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("https://apinetcoreenquiry.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// SWAGGER
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

// MIDDLEWARE
app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

// Abilita Swagger se necessario
app.UseSwagger();
app.UseSwaggerUI(c =>
{
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enquiry API V1");
     c.RoutePrefix = "swagger";
});

app.MapControllers();


// PORTA RENDER
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

//app.MapGet("/", () => "Api is running 🚀");   

app.Run();
