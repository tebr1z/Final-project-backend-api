using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// MVC servisini ekle
builder.Services.AddControllersWithViews();

// HttpClient servisini ekle (Hata buradan kaynaklanýyordu)
builder.Services.AddHttpClient("LmsApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7162/"); // API'nin base adresini doðru ayarla
});


// Authentication servisini ekle
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware sýrasý önemli!
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication ve Authorization Middleware'leri burada eklenmeli
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
