using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;

namespace TestWinnicode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // konfigurasi DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 33))
            ));

            // konfigurasi authentication
            builder.Services.AddAuthentication("CookieAuth")
            .AddCookie("CookieAuth", options =>
            {
                options.LoginPath = "/Account/Login";
            });

            builder.Services.AddControllersWithViews();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");


            app.Run();
        }
    }
}
