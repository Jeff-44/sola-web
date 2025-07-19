using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using ApplicationCore.Settings;
using ApplicationCore.Utils;
using OpenHtmlToPdf;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sola_Web.Services;

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
builder.Services.AddScoped<IPdfService, OpenHtmlToPdfService>();
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
