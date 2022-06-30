using IdentityTutBhumMVC.DataBase;
using IdentityTutBhumMVC.Service;
using Microsoft.AspNetCore.Authentication;
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

//Adding the Exerenal Login Setup
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1860438640832192";
    options.AppSecret = "95291c1290ee0f754103647d8fd5f70f";
});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    //options.ClaimActions.MapJsonKey("urn:google:picture", "picture");
});

//Application cookies for the Unautorixe page
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

//Adding the application policy
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("AdminAndUser", policy => policy.RequireRole("Admin").RequireRole("User"));
    opt.AddPolicy("Admin_CreateAccess", policy => policy.RequireRole("Admin").RequireClaim("create", "True")); //require claim can be one
    opt.AddPolicy("Admin_Create_Edit_DeleteAccess", policy => policy.RequireRole("Admin").RequireClaim("create", "True")
    .RequireClaim("Edit", "True").RequireClaim("Delete", "True"));

});

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
