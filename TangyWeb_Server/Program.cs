using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using Tangy_Business.Repository;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Data;
using TangyWeb_Server.Service.IService;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1hXk5Hd0BLVGpAblJ3T2ZQdVt5ZDU7a15RRnVfRFxiSHhQcUdiW3tZdQ==;Mgo+DSMBPh8sVXJ1S0R+X1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jT39Sd0NgXn1fcnJTQw==;ORg4AjUWIQA/Gnt2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhSd0RlWnhccXZdR2U=;MjM4MjkxOEAzMjMxMmUzMDJlMzBpeDdhYUo4WHJFaHg1cVFKait3S3l2SEFmUWQ0d2w4Q3ZsTVhNK2EwMlRvPQ==;MjM4MjkxOUAzMjMxMmUzMDJlMzBEa2FyNTJhZHJFekJnWEtvdDVwZU55VzR3Ti9nanNadEh0TUZTWVh2UlE4PQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5Vd0RiX3xZcnVWQGBZ;MjM4MjkyMUAzMjMxMmUzMDJlMzBvaE9zZGpkQjVVeG9UVWFuajFUMEZadlMvSGlTVlZIREgwSkIzTG5uRjFvPQ==;MjM4MjkyMkAzMjMxMmUzMDJlMzBKSmJneTNPelRVd1lxL1lrUHNuU2UvSTZBRTBHU3RHVm1SU3RiYXJ0MGlRPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhSd0RlWnhccXBWRGU=;MjM4MjkyNEAzMjMxMmUzMDJlMzBnd1pqeUFXeE50bGc5VC9WOS96TE5lVit1OFFSSjRnWTUzeU44bzNuUUJrPQ==;MjM4MjkyNUAzMjMxMmUzMDJlMzBVak9KMFRJKy85UUJab05ZS2ZTQ2F0K0ZvTExRYkRCM3pVL2crVmdFVmZNPQ==;MjM4MjkyNkAzMjMxMmUzMDJlMzBvaE9zZGpkQjVVeG9UVWFuajFUMEZadlMvSGlTVlZIREgwSkIzTG5uRjFvPQ==");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

//  Register the ApplicationDbContext class with the DI container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the Repositories with the DI container
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
builder.Services.AddScoped<IFileUpload, FileUpload>();

// Register AutoMapper with the DI container
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
