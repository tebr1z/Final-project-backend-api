using LmsApiApp.Application.Exceptions;
using LmsApiApp.Presentation;
using LmsApiApp.Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.Register(config);



var app = builder.Build();
















app.UseCors("AllowAll");
app.MapHub<TimerHub>("/testHub");

app.MapHub<ChatHub>("/chathub");
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Swagger her ortamda aktif olsun
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = ""; // Swagger UI'yi kök dizinde göstermek için
});


app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
