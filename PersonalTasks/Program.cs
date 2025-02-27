using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalTasks;
using PersonalTasks.Auth.Services;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Sevices;
using PointOfSale.Shared.Settings;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// DATABASE
builder.Services.AddDbContext<ContextDb>(op => op
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));

// DI SERVICES
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Authentication
var jwtSettings = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secret = jwtSettings.GetValue<string>("SecretKey")
             ?? throw new Exception("JWT SecretKey not found");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

// Authorization
builder.Services.AddAuthorization();


//MAPPER CONFIGURATION
var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);



// Configure the HTTP request pipeline.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
