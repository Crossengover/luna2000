using luna2000.Converters;
using luna2000.Data;
using luna2000.Logs;
using luna2000.Logs.Impl;
using luna2000.MapperProfiles;
using luna2000.Middlewares;
using luna2000.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace luna2000;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

        #region Logs
        builder.Services.AddScoped<ILogMessageGeneratorFactory, LogMessageGeneratorFactory>();
        builder.Services.AddTransient<ILogMessageGenerator, PhotoEntityLogMessageGenerator>();
        builder.Services.AddTransient<ILogMessageGenerator, DriverEntityLogMessageGenerator>();
        builder.Services.AddTransient<ILogMessageGenerator, CarEntityLogMessageGenerator>();
        builder.Services.AddTransient<ILogMessageGenerator, CarRentalEntityLogMessageGenerator>();
        #endregion

        builder.Services.AddDbContext<LunaDbContext>(ServiceLifetime.Scoped);
        builder.Services.AddScoped<IDbContextFactory<LunaDbContext>, DbContextFactory>();
        builder.Services.AddScoped<IFileStorage, FileStorage>();
        builder.Services.AddScoped<IDeductRentService, DeductRentService>();
        builder.Services.AddAutoMapper(expression => expression.AddProfiles(new []
        {
            new EntityProfiles()
        }));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Events.OnRedirectToLogin += context => {
                context.HttpContext.Response.Redirect("/login");
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied += context => {
                context.HttpContext.Response.Redirect("/login");
                return Task.CompletedTask;
            };

            options.LoginPath = new PathString("/login");
            options.ReturnUrlParameter = "link";
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseMiddleware<IpRestrictionMiddleware>();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "files")),
            RequestPath = ""
        });

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}