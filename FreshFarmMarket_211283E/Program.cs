using FreshFarmMarket_211283E.Google;
using FreshFarmMarket_211283E.Services;
using FreshFarmMarket_211283E.Models;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using FreshFarmMarket_211283E.DataContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// OTP
builder.Services.Configure<SMSoptions>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddScoped<MessageService>();

//Google ReCaptcha
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));
builder.Services.AddTransient(typeof(GoogleCaptchaService));

// Log User Activities
builder.Services.AddScoped<LogServices>();

//Google Login
builder.Services.AddAuthentication()
.AddGoogle(options =>
{
	var config = builder.Configuration.GetSection("Authentication").Get<GoogleLoginConfig>();
	options.ClientId = config.ClientId;
	options.ClientSecret = config.ClientSecret;
});

// builder.Services.Configure<>

builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<PhoneNumberTokenProvider<ApplicationUser>>("PhoneSMS");



// Secure CreaditCardNumber
builder.Services.AddDataProtection()
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });


// Session Management
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "FreshFarmMarket_211283E";
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configure
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
	// options.SlidingExpiration = true;
	options.SlidingExpiration = false;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 1;


    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;


});



builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

// Customed Error Message
app.UseStatusCodePages( context =>
{
	context.HttpContext.Response.ContentType = "text/plain";

	switch (context.HttpContext.Response.StatusCode)
	{
		case 404:
			context.HttpContext.Response.Redirect("/ErrorPages/Error404");
			break;
		case 403:
			context.HttpContext.Response.Redirect("/Error403");
			break;
		default:
			context.HttpContext.Response.Redirect("/Error");
			break;
	}

	return Task.CompletedTask;

});



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
