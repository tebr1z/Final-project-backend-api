using FluentValidation.AspNetCore;
using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Implementations;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Mappers;
using LmsApiApp.Application.Profiles;
using LmsApiApp.Application.Repositories;
using LmsApiApp.Application.Services;
using LmsApiApp.Application.Settings;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
namespace LmsApiApp.Presentation
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterDtoValidator>());

            




            services.AddCors(options =>
             {
                 options.AddPolicy("AllowAll",
                     builder =>
                     {
                         builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                     });
             });

            // Add services to the container
            services.AddControllers();




            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // DbContext configuration
            services.AddDbContext<LmsApiDbContext>(opt =>
             {
                 opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
             });


            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength = 6;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<LmsApiDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = config["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"];
                googleOptions.CallbackPath = "/auth/google-callback";
            })


                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });






            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Huquq Api",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });











            // Dependency Injection (DI) for Services and Repositories
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ICourseServices, CourseService>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAssignmentService, AssignmentService>();

            services.Configure<JwtSetting>(config.GetSection("Jwt"));



            // AutoMapper configuration
            services.AddAutoMapper(typeof(MapperProfiles));
            services.AddAutoMapper(typeof(GroupProfile));

        }
    }
}
