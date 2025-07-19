using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using ApplicationCore.Settings;
using ApplicationCore.Utils;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;
using Infrastructure.Data;
using Infrastructure.Implementations.Repositories;
using Infrastructure.Services;
using Infrastructure.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sola_Web.Services;
using Sola_Web.Helpers;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContext<SolaContext>(options => 
{
    options.UseNpgsql(connectionstring);
});

builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SolaContext>();

// Configure WkHtmlToPdf converter


// Load native wkhtmltopdf library before using DinkToPdf
NativeLibraryLoader.Load(nativeLibPath);


// Register repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ISolaServicesRepository, SolaServicesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
// Register services
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IPdfService, HtmlToPdfService>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IEmailAttachmentSender, EmailSender>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope()) 
{
    var services = scope.ServiceProvider;
    var _context = services.GetRequiredService<SolaContext>();
    //context.Database.EnsureDeleted();
    //context.Database.EnsureCreated();
    _context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
