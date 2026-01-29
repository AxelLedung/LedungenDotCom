using LedungenDotCom.Models;
using LedungenDotCom.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEmailService, GmailEmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


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

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Get your EmailSettings
var emailSettings = app.Services.GetRequiredService<IOptions<EmailSettings>>().Value;

// Log safely (never log password!)
logger.LogInformation("Email settings loaded: Host={Host}, Port={Port}, From={From}, Username={Username}, Password={Password}",
    emailSettings.Host, emailSettings.Port, emailSettings.From, emailSettings.Username, emailSettings.Password);

// Optional: check if password is present
logger.LogInformation("Password is configured: {HasPassword}", !string.IsNullOrEmpty(emailSettings.Password));

app.Run();