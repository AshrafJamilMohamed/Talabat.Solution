using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Identity.IdentityDataSeed;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region  Add services to the container.

            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            AddApplicationServices.AddServices(webApplicationBuilder.Services);

            // Applying Dependancy Injection For StoreContext

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
                 {
                     options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));

                 });

            // Applying Dependancy Injection For AppIdentitiyDbContext
            webApplicationBuilder.Services.AddDbContext<AppIdentitiyDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));

            });

            // Allow Dependancy Injection For usermanager,signinmanager,rolemanager
            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                                          .AddEntityFrameworkStores<AppIdentitiyDbContext>();

            // Allow the Angular Project To consume the EndPoints  
            webApplicationBuilder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", o =>
                {
                    o.AllowAnyHeader().AllowAnyMethod().WithOrigins(webApplicationBuilder.Configuration["FrontBaseUrl"]);
                });
            });


            // Set Default Token Validation

            webApplicationBuilder.Services.AddTokenValidation(webApplicationBuilder.Configuration);
            #endregion

            #region Redis

            // Allow DependancyInjection For Redis Service
            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>(
                Options =>
                {
                    var Connection = webApplicationBuilder.Configuration.GetConnectionString("redis");
                    return ConnectionMultiplexer.Connect(Connection);
                }


            );

            #endregion

             // Add Swagger
            webApplicationBuilder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Talabat APIs", Version = "v1" });

                c.CustomSchemaIds(type =>
                {
                    if (type == typeof(Talabat.Core.Entities.OrderAggregation.Address))
                        return "OrderAggregationAddress";
                    if (type == typeof(Talabat.Core.Entities.Identity.Address))
                        return "IdentityAddress";
                    return type.FullName;
                });
            });

            var app = webApplicationBuilder.Build();

            #region Update and Seeding Database 

            using var Scope = app.Services.CreateScope();

            var Service = Scope.ServiceProvider;

            var DbContext = Service.GetRequiredService<StoreContext>();
            var IdentityDbContext = Service.GetRequiredService<AppIdentitiyDbContext>();
            var userManager = Service.GetRequiredService<UserManager<ApplicationUser>>();
            var LoggerFactory = Service.GetRequiredService<ILoggerFactory>();

            try
            {
                await DbContext.Database.MigrateAsync(); // Update Database
                await IdentityDbContext.Database.MigrateAsync(); // Update Database
                await DataSeeding.SeedData(DbContext);   // DataSeeding
                await IdentityDbContextDataSeed.DataSeedAsync(userManager);   // IdentityDataSeeding

            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, ex.Message);

            }

            #endregion

            #region  Configure the HTTP request pipeline.

            // Must be the first Middleware 
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
