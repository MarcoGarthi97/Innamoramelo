using Innamoramelo;
using Innamoramelo.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var urlAPI = builder.Configuration["UrlAPI"];
var urlAdminCredentials = builder.Configuration["AdminCredentials"];

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

startup.Configure(app, app.Environment);

app.MapHub<ChatHub>("/chatHub");

app.Run();
