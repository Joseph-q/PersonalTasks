using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalTasks;
using PersonalTasks.Auth.Services;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Sevices;
using PointOfSale.Shared.Settings;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var policyCors = "AllowAll";

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(policyCors, policy =>
    {
        policy.AllowAnyOrigin()   // Permite solicitudes desde cualquier origen
              .AllowAnyMethod()   // Permite cualquier método (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader();  // Permite cualquier cabecera
    });
});
// DATABASE
string? connString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");


if (string.IsNullOrEmpty(connString))
{
    throw new Exception("Connection string not found");
}


builder.Services.AddDbContext<ContextDb>(op => op
    .UseSqlServer(connString)
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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa solo en token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

var app = builder.Build();

// Configura el pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyCors);

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
