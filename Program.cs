using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Thêm HttpContextAccessor để dùng HttpContext.Session, ViewBag, v.v.
builder.Services.AddHttpContextAccessor();

// ✅ Cấu hình Session (giữ đăng nhập lâu dài)
builder.Services.AddSession(options =>
{
    // ✅ Giữ session 30 ngày
    options.IdleTimeout = TimeSpan.FromDays(120);

    // ✅ Cookie bắt buộc và chỉ dùng trong trình duyệt (bảo mật hơn)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    // ✅ Cho phép cookie session hoạt động ổn định trên HTTPS
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// ✅ Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
// ✅ Thêm MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⚠️ Session phải bật TRƯỚC Authorization
app.UseSession();
app.UseAuthorization();

// ✅ Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
