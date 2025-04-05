var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Thời gian session hết hạn
    options.Cookie.HttpOnly = true;                   // Chỉ truy cập cookie từ phía server
    options.Cookie.IsEssential = true;                // Đánh dấu cookie là quan trọng (vì lý do GDPR)
});

var app = builder.Build();

// Use session middleware
app.UseSession();

// Use routing middleware
app.UseRouting();

// Add authentication and authorization if needed (e.g. for login/logout)
// app.UseAuthentication(); // Enable if authentication is needed
// app.UseAuthorization();  // Enable if authorization is needed

// Ensure the routing works for controllers (including Auth controller)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"); // Đảm bảo action mặc định là Login

// Map controller routes for other pages (e.g., Teacher/ManageGrade)
app.MapControllerRoute(
    name: "manageGrade",
    pattern: "Teacher/ManageGrade", // URL cho trang ManageGrade
    defaults: new { controller = "Teacher", action = "ManageGrade" });

app.Run();
