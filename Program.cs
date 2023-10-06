using AspNetIdentityCoreApp.Web.ClaimProvider;
using AspNetIdentityCoreApp.Web.CustomValidations;
using AspNetIdentityCoreApp.Web.Extenisons;
using AspNetIdentityCoreApp.Web.Models;
using AspNetIdentityCoreApp.Web.PermissionsRoot;
using AspNetIdentityCoreApp.Web.Requirements;
using AspNetIdentityCoreApp.Web.Seeds;
using AspNetIdentityCoreApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.Configure<EmailService>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddIdentityWithExt();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireReqirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequiremntHandler>();

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AnkaraPolicy", opt =>
    {
        opt.RequireClaim("city", "Ankara");
    });
    option.AddPolicy("ExchangePolicy", opt =>
    {
        opt.AddRequirements(new ExchangeExpireRequirenment());
    });
    option.AddPolicy("ViolencePolicy", opt =>
    {
        opt.AddRequirements(new ViolenceRequirement());
    });
    option.AddPolicy("OrderPermissionReadAndDelete", opt =>
    {
        opt.RequireClaim("Permission",Permissions.Order.Read, Permissions.Order.Delete, Permissions.Stock.Delete);
    });

    option.AddPolicy("OrderDelete", opt =>
    {
        opt.RequireClaim("Permission", Permissions.Order.Delete);
    });
    option.AddPolicy("OrderCreate", opt =>
    {
        opt.RequireClaim("Permission", Permissions.Order.Create);
    });
    option.AddPolicy("OrderUpdate", opt =>
    {
        opt.RequireClaim("Permission", Permissions.Order.Update);
    });
});
builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "UdemyCookie";

    opt.Cookie = cookieBuilder;
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.LogoutPath = new PathString("/Member/LogOut");
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    opt.ExpireTimeSpan = TimeSpan.FromDays(10);
    opt.SlidingExpiration = true;
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    await PermissionSeed.Seed(roleManager); 
}
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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
