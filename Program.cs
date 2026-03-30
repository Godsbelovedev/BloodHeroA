using BloodHeroA.Application.Services.BackgroundJob;
using BloodHeroA.Application.Services.Implementations;
using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.Data;
using BloodHeroA.Models.Entities;
using BloodHeroA.Repositories.Implementation;
using BloodHeroA.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(i => i.UseMySql
(builder.Configuration.GetConnectionString("DbString"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbString")
)));

builder.Services.Configure<EmailSettings>(
builder.Configuration.GetSection("EmailSettings"));



builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRecipientOrganizationService, RecipientOrgaizationService>();
builder.Services.AddScoped<IBankingOrganizationService, BankingOrganizationService>();
builder.Services.AddScoped<IDonorOrganizationService, DonorOrganizationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IDonationRequestService, DonationRequestService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBloodTestResultService, BloodTestResultService>();
builder.Services.AddScoped<IBloodStorageService, BloodStorageService>();
builder.Services.AddScoped<IReleasedBloodService, ReleasedBloodService>();
builder.Services.AddScoped<IBloodInventoryService, BloodInventoryService>();


builder.Services.AddHostedService<BloodExpiryDateChecker>();



builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRecipientOrganizationRepository, RecipientOrganizationRepository>();
builder.Services.AddScoped<IBankingOrganizationRepository, BankingOrganizationRepository>();
builder.Services.AddScoped<IDonorOrganizationRepository, DonorOrganizationRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();
builder.Services.AddScoped<IDonationRequestRepository, DonationRequestRepository>();
builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddScoped<IBloodTestResultRepository, BloodTestResultRepository>();
builder.Services.AddScoped<IBloodStorageRepository, BloodStorageRepository>();
builder.Services.AddScoped<IReleasedBloodRepository, ReleasedBloodRepository>();
builder.Services.AddScoped<IBloodInventoryRepository, BloodInventoryRepository>();
builder.Services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        //options.LogoutPath = "/Users/Logout";
        options.AccessDeniedPath = "/Users/AccessDenied";
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
