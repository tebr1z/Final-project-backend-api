using LmsApiApp.Application.Exceptions;
using LmsApiApp.Application.Implementations;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Profiles;
using LmsApiApp.Application.Repositories;
using LmsApiApp.Application.Services;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext configuration
builder.Services.AddDbContext<LmsApiDbContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

// Dependency Injection (DI) for Services and Repositories
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ICourseServices, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();


builder.Services.AddScoped<IAssignmentService, AssignmentService>();

// Add UserManager and other Identity services
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<LmsApiDbContext>()
    .AddDefaultTokenProviders();


// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(MapperProfiles));

// Exception middleware registration


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Hata sayfasý
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
