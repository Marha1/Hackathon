using Application.Dtos.GoogleReCaptchaDto;
using Application.Mapping;
using Application.Middleware;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Infrastructure;
using Infrastructure.DAL.Configurations;
using Infrastructure.DAL.Interfaces;
using Infrastructure.DAL.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<AplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdsRepository, AdsRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdsService, AdsService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IGoogleReCaptchaService, GoogleReCaptchaService>();
builder.Services.Configure<GoogleReCaptchaSettings>(builder.Configuration.GetSection("GoogleReCaptchaSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddHttpClient("GoogleReCaptchaSettings",
    c => { c.BaseAddress = new Uri("https://www.google.com/recaptcha/api/"); });
builder.Services.AddAutoMapper(typeof(AdsMappingProfile), typeof(UserMappingProfile));
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseMiddleware<AdsValidationMiddleware>();
app.UseMiddleware<UserValidationMiddleware>();
app.UseMiddleware<ImageValidationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();