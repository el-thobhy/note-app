using Administrator.Services;
using Administrator.Services.auth_project.Services;

var builder = WebApplication.CreateBuilder(args);

// Menambahkan session services
builder.Services.AddDistributedMemoryCache(); // Menyimpan session di memori
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout session 30 menit
    options.Cookie.HttpOnly = true; // Menjamin hanya bisa diakses di server
    options.Cookie.IsEssential = true; // Pastikan cookie selalu dikirim meskipun tidak ada interaksi
});
//services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Gunakan session middleware sebelum routing
app.UseSession(); 

app.UseStaticFiles();

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.Run();
