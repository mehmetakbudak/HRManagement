using Chinook.Data.Repository;
using Chinook.Data;
using Chinook.Storage.Models;
using Chinook.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Chinook.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<ChinookContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var appSettingsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtModel>(appSettingsSection);
var appSettings = appSettingsSection.Get<JwtModel>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddMvc()
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();
builder.Services.AddScoped<INoteCategoryService, NoteCategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ITitleService, TitleService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddCors();
builder.Services.AddControllers(mvcOptions =>
                mvcOptions.EnableEndpointRouting = false);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

string[] corsDomains = builder.Configuration["CorsDomains:Domains"].Split(",");
app.UseCors(options => options.WithOrigins(corsDomains)
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .AllowAnyHeader());
app.UseRouting();
app.ErrorHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
