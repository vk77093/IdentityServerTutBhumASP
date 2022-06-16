using IdentityTutBhumMVC.DataBase;
using IdentityTutBhumMVC.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionstrings = builder.Configuration.GetConnectionString("databaseConnection");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(connectionstrings);
});
//Adding the Identity Service
builder.Services.AddIdentity<IdentityUser,IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().
    //Adding for forgot password token generation
    AddDefaultTokenProviders()
    ;

//Adding the EMail sender service for forgot password
builder.Services.AddTransient<IEmailSender, MailJetEmailSender>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
