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
using LmsApiApp.Presentation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                         builder.WithOrigins("http://141.98.112.193", "https://hashimovtabriz.com.tr", "http://localhost:5500")

                            .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
                     });
             });


            services.AddControllers();




            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

           
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


                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                      
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken; 
                        }
                        return Task.CompletedTask;
                    }
                };








            });






            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LMS api ",
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
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.Configure<JwtSetting>(config.GetSection("Jwt"));
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISignalRService, SignalRService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddSignalR();
            // AutoMapper configuration
            services.AddAutoMapper(typeof(MapperProfiles));
            services.AddAutoMapper(typeof(GroupProfile));

        }
    }
}
