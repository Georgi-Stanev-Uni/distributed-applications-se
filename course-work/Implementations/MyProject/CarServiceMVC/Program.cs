using CarServiceMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CarServiceMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            // Add EF Core and connection string
            builder.Services.AddDbContext<CarServiceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Car Service API",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            // Middleware pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // serves wwwroot content
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            // Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Service API v1");
                c.RoutePrefix = "swagger"; // URL will be /swagger
            });

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=LogIn}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
