using _2210900059_PTQ_DATN.Models;
using Microsoft.EntityFrameworkCore;

namespace _2210900059_PTQ_DATN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            // DbContext (GIỮ NGUYÊN)
            var conectionString = builder.Configuration.GetConnectionString("LeskinConnection");
            builder.Services.AddDbContext<LeSkinDbContext>(
                x => x.UseSqlServer(conectionString)
            );

            // >>> THÊM: Session (BẮT BUỘC cho đăng nhập)
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian hết hạn session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
           
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
         

            app.UseAuthorization();


            // Area route (ADMIN)
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Default route (USER)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
