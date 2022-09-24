using Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// set environment for dev
string DevelopmentEnvironment = "office";

string defaultConnection;
if (DevelopmentEnvironment == "home") {
    defaultConnection = builder.Configuration.GetConnectionString("HomeConnection");
} else {
    defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
}

if (string.IsNullOrEmpty(defaultConnection)) throw new Exception("Missing connection string");

builder.Services.AddFluentNHibernate(defaultConnection, DevelopmentEnvironment);

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

app.Run();
