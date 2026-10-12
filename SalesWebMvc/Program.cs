using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;
namespace SalesWebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("SalesWebMvcContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found.");

            builder.Services.AddDbContext<SalesWebMvcContext>(options => options.UseMySQL(connectionString));

            // Registra o SeedService na injeção de dependência
            builder.Services.AddScoped<SeedService>();

            // Registra o SellerService na injeção de dependência
            builder.Services.AddScoped<SellerService>();

            // Registra o DepartmentService na injeção de dependência
            builder.Services.AddScoped<DepartmentService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            var enUS = new CultureInfo("en-US");

            var localizationOptions = new RequestLocalizationOptions {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(enUS),
                SupportedCultures = new List<CultureInfo> { enUS },
                SupportedUICultures = new List<CultureInfo> { enUS }
            };

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
                    seedService.Seed();
                }
            } 
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization(localizationOptions);
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
