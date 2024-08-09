using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;
using Talabat.Repository;
using Talabat.Repository.BasketRepository;
using Talabat.Service.AuthToken;
using Talabat.Service.OrderServiceModule;
using Talabat.Service.PaymentService;

namespace Talabat.APIs.Extensions
{
    public static class AddApplicationServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            services.AddAutoMapper(typeof(MappingProfiles));

            // Configure The Service of Validation Error Repsonse 400
            // if the model is not vaild it will handel it 

            services.Configure<ApiBehaviorOptions>(options =>
             {
                 options.InvalidModelStateResponseFactory = (actionContext) =>
                 {
                     var _Errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                             .SelectMany(E => E.Value.Errors)
                                             .Select(M => M.ErrorMessage)
                                             .ToList();

                     var Response = new ApiValidationErrorRepsonse()
                     {
                         Errors = _Errors
                     };
                     return new BadRequestObjectResult(Response);
                 };
             });

         
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            return services;
        }



        public static IServiceCollection AddTokenValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(O =>
            {
                O.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                O.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(O =>
            {
                O.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssure"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudiance"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"])),
                    ClockSkew = TimeSpan.Zero


                };
            });


            return services;


        }

    }

}
