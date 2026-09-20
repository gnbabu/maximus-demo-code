using MainBoldReportsAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
DIRegistrar.RegisterServices(builder.Services, builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );
    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
                new string[] { }
            }
        }
    );
});
builder.Services.AddCors(o =>
{ 
o.AddPolicy("NewPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
    string[] s = builder.Configuration["Origins"].Split(",");
    o.AddPolicy("RestrictPolicy",
    builder =>
    {
        builder.WithOrigins(s)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    }); 
}) ;
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

//Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName.Equals("MES") || app.Environment.EnvironmentName.Equals("BUILD01") || app.Environment.EnvironmentName.Equals("INT01") || app.Environment.EnvironmentName.Equals("PERF") || app.Environment.EnvironmentName.Equals("INT02") || app.Environment.EnvironmentName.Equals("UAT") || app.Environment.EnvironmentName.Equals("E2E") || app.Environment.EnvironmentName.Equals("TRN"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Load configuration from appsettings.json
//app.UseSwagger();
//app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("RestrictPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();
app.MapControllers();

app.Run();
