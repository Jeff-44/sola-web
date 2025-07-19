using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using ApplicationCore.Settings;
using ApplicationCore.Utils;
using DinkToPdf;
using DinkToPdf.Contracts;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sola_Web.Services;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SolaContext>(options => 
{
    options.UseNpgsql(connectionstring);
});

builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SolaContext>();

string nativeLibPath;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    nativeLibPath = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", "win-x64", "native", "libwkhtmltox.dll");
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    nativeLibPath = Path.Combine(Directory.GetCurrentDirectory(), "runtimes", "linux-x64", "native", "libwkhtmltox.so");
}
else
{
    throw new PlatformNotSupportedException("Only Windows and Linux are supported.");
}

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(nativeLibPath);


// Register repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ISolaServicesRepository, SolaServicesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Register services
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
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
    var context = services.GetRequiredService<SolaContext>();
    //context.Database.EnsureDeleted();
    //context.Database.EnsureCreated();
    context.Database.Migrate();
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
