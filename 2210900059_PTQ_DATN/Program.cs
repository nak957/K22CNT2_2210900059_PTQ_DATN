using _2210900059_PTQ_DATN.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace _2210900059_PTQ_DATN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Thêm dịch vụ MVC
            builder.Services.AddControllersWithViews();

            // DbContext (GIỮ NGUYÊN)
            var conectionString = builder.Configuration.GetConnectionString("LeskinConnection");
            builder.Services.AddDbContext<LeSkinDbContext>(
                x => x.UseSqlServer(conectionString)
            );

            // >>> THÊM: Session (BẮT BUỘC cho đăng nhập)
            // Lưu trạng thái session trong bộ nhớ (đơn giản, dev/test). Thời gian timeout 1 giờ.
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
           
            // Authentication (Cookie)
            // Đăng ký middleware xác thực cookie để HttpContext.SignInAsync hoạt động
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/TaiKhoan/Login"; // khi chưa đăng nhập sẽ redirect tới đây
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Cookie.Name = "LeSkinAuth";
                });

            var app = builder.Build();

            // Cấu hình pipeline middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Quan trọng: đăng ký Authentication trước Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Kích hoạt Session (phải trước khi endpoints thực thi nếu cần dùng session trong controller)
            app.UseSession();


            // Route cho area (ADMIN)
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Route mặc định (USER)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
