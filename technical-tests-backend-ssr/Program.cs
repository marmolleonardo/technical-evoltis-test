using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// FluentValidation automatic model validation
builder.Services.AddFluentValidationAutoValidation();
// Optionally enable client-side adapters if needed by MVC views
builder.Services.AddFluentValidationClientsideAdapters();
// Register validators explicitly
builder.Services.AddScoped<FluentValidation.IValidator<technical_tests_backend_ssr.Dtos.ProductCreateDto>, technical_tests_backend_ssr.Validators.ProductCreateDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<technical_tests_backend_ssr.Dtos.ProductUpdateDto>, technical_tests_backend_ssr.Validators.ProductUpdateDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure Entity Framework Core with MySQL (Pomelo)
// Prefer the MYSQL_CONNECTION_STRING environment variable (includes server, database, user, password)
var envConn = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
string connectionString;
if (!string.IsNullOrWhiteSpace(envConn))
{
    connectionString = envConn;
}
else
{
    var baseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection in appsettings.json or the MYSQL_CONNECTION_STRING environment variable.");
    connectionString = baseConnectionString; // We do not append a password here.
}

// Avoid AutoDetect at design time so 'dotnet ef migrations' doesn't try to connect to the DB.
// You can override with the MYSQL_SERVER_VERSION environment variable (e.g. "8.0.36-mysql").
var serverVersionString = Environment.GetEnvironmentVariable("MYSQL_SERVER_VERSION");
var serverVersion = !string.IsNullOrWhiteSpace(serverVersionString)
    ? ServerVersion.Parse(serverVersionString)
    : ServerVersion.Parse("8.0.36-mysql");

builder.Services.AddDbContext<TechnicalTestDbContext>(opt =>
    opt.UseMySql(connectionString, serverVersion)
);

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configure CORS to allow Angular app (adjust origins as needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://localhost:4200"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
