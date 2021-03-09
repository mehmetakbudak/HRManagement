using Chinook.Data;
using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Chinook.Service;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace Chinook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChinookContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            var appSettingsSection = Configuration.GetSection("Jwt");
            services.Configure<JwtModel>(appSettingsSection);
            var appSettings = appSettingsSection.Get<JwtModel>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddScoped<INoteCategoryService, NoteCategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPageService, PageService>();

            services.AddCors();

            services.AddOData();
            services.AddControllers(mvcOptions =>
                            mvcOptions.EnableEndpointRouting = false);

            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddMvc(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            }).AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !System.IO.Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            string[] corsDomains = Configuration["CorsDomains:Domains"].Split(",");
            app.UseCors(options => options.WithOrigins(corsDomains)
                                          .AllowAnyMethod()
                                          .AllowCredentials()
                                          .AllowAnyHeader());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Count().Filter().OrderBy().MaxTop(100).SkipToken().Build();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            builder.EntitySet<NoteCategoryModel>("NoteCategory");
            builder.EntitySet<NoteModel>("Note");

            var lookup = builder.EntitySet<Lookup>("Lookup");
            lookup.EntityType.Collection.Function("Get").ReturnsCollection<Lookup>();
            lookup.EntityType.Collection.Function("Provinces").ReturnsCollection<Province>();
            lookup.EntityType.Collection.Function("Cities").ReturnsCollection<City>();

            builder.EntitySet<BlogCategory>("FeBlogCategory");
            builder.EntitySet<Blog>("FeBlog");
            builder.EntitySet<Page>("FePage");
            builder.EntitySet<MenuModel>("FeMenu");


            return builder.GetEdmModel();
        }
    }
}
