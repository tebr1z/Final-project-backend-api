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

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
