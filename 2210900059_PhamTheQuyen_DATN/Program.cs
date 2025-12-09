using Microsoft.EntityFrameworkCore;
using _221090005_PhamTheQuyen_DATN.Models;

namespace _221090005_PhamTheQuyen_DATN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         
            builder.Services.AddDbContext<LeSkinDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("LeSkinDB")
                )
            );
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}

