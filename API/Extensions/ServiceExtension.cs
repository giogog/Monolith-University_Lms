using Domain;
using Application.Mapping;
using Contracts;
using Domain.Models;
using Infrastructure;
using Infrastructure.Auth;
using Infrastructure.DataConnection;
using Infrastructure.Email;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using Application.Services;
using Application.CQRS.Commands;

namespace API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)=>
            services.AddDbContext<DomainDataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options =>
                {
                    options.MigrationsAssembly("API");
                }));


        public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration) =>
            services.AddScoped<MongoDbContext>(sp =>
            {
                var client = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
                return new MongoDbContext(client, configuration["MongoDb:DatabaseName"]); ;
            });
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
        {

            string TokenKey = config["Jwt:Key"];

            services.AddIdentity<User, Role>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;


            }).AddEntityFrameworkStores<DomainDataContext>().AddDefaultTokenProviders();



            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our SignalR hubs
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/applicantHub") || path.StartsWithSegments("/enrollmentHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection service) =>
            service.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureAutomapper(this IServiceCollection services) => 
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

        public static void ConfigureMediatR(this IServiceCollection services) =>
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(StudentApplicationCommand).Assembly));

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:5003")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials());
            });
    }
}
