using Microsoft.AspNetCore.Authentication.Cookies;
using PlcCreatorSystem_WEB;
using PlcCreatorSystem_WEB.Services;
using PlcCreatorSystem_WEB.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddHttpClient<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddHttpClient<IPlcService, PlcService>();
builder.Services.AddScoped<IPlcService, PlcService>();

builder.Services.AddHttpClient<IHmiService, HmiService>();
builder.Services.AddScoped<IHmiService, HmiService>();

builder.Services.AddHttpClient<IUserService, AuthService>();
builder.Services.AddScoped<IUserService, AuthService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.SlidingExpiration = true;
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//add client gRPC
builder.Services.AddHttpClient("PLC_CREATOR_SYSTEM_API", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:Creator_API"]!);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
