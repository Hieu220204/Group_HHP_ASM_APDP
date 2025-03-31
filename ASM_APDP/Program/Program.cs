var builder = WebApplication.CreateBuilder(args);

// Tắt Browser Link nếu không cần
builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
});

// Cấu hình session và các middleware quan trọng
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// **TẮT BROWSER LINK**
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/_framework"), appBuilder =>
{
    appBuilder.UseSession();
});

// Cấu hình Middleware cơ bản
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
