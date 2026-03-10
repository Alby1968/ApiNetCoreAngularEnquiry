using ApiNetCoreAngular.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            
            //builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
            builder.WithOrigins("https://enquiryapiangular.netlify.app")
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();                         // SWAGGER
builder.Services.AddSwaggerGen(options =>                           // SWAGGER
{
    options.SwaggerDoc("Enquiry", new OpenApiInfo
    {
        Title = "Enquiry Api",
        
        Version = "V1"
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())                                          // SWAGGER
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Enquiry/swagger.json", "Enquiry API");
        options.RoutePrefix = "swagger"; // default

    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
