using Application.Mapping;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Enities;
using Domain.Primitives;
using Infrastrucure;
using Infrastrucure.DAL.Interfaces;
using Infrastrucure.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using reCAPTCHA.AspNetCore;
using SixLabors.ImageSharp;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<AplicationContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<IAdsRepository<Ads>, AdsRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdsService, AdsService>();
builder.Services.AddControllers();
builder.Services.AddScoped<IGoogleReCaptchaService, GoogleReCaptchaService>();
builder.Services.Configure<GoogleReCaptchaSettings>(builder.Configuration.GetSection("GoogleReCaptchaSettings"));
builder.Services.AddHttpClient("GoogleReCaptchaSettings", c =>
{
    c.BaseAddress = new Uri("https://www.google.com/recaptcha/api/");
});
builder.Services.AddAutoMapper(typeof(AdsMappingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddControllers();
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
app.MapControllers();
app.Run();