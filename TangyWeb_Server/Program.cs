using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using Tangy_Business.Repository;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Data;
using TangyWeb_Server.Service.IService;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjMwMDg3NEAzMjMxMmUzMDJlMzBaZ1RNenR0QVZVNEd2QVdYSThvd2d6aUwvNVYrQStrek9FTjRHeTlqZzk4PQ==;Mgo+DSMBaFt+QHJqVk1mQ1BEaV1CX2BZf1F8QGFTe15gFChNYlxTR3ZZQl5iS3tRckVlWHdc;Mgo+DSMBMAY9C3t2VFhiQlJPcEBDXXxLflF1VWJbdVtyflFGcDwsT3RfQF5jT35Qd0dmW3tedndRRA==;Mgo+DSMBPh8sVXJ1S0R+X1pCaV5CQmFJfFBmRGFTelx6cFVWACFaRnZdQV1lSX1SdEBhXX9YdHxX;MjMwMDg3OEAzMjMxMmUzMDJlMzBsTmRiTGFoaHF2czdZd2JEWmR4TGE0Rmx2Q3JwYW42R2NQVDhDd1liM1NvPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfVldXGNWfFN0RnNYfVR1dl9CYEwxOX1dQl9gSXhTdURhXH1acHJdQ2I=;ORg4AjUWIQA/Gnt2VFhiQlJPcEBDXXxLflF1VWJbdVtyflFGcDwsT3RfQF5jT35Qd0dmW3ted3RURA==;MjMwMDg4MUAzMjMxMmUzMDJlMzBNSUNJK2lRa3gvQ3FmSWJaTjNRTkxyWFNYSnRGUEVYODdURDJkeTJVcmxnPQ==;MjMwMDg4MkAzMjMxMmUzMDJlMzBsdkhyMnIzaU5RL0ZLZzRYdk5WY3BBMVpaY0FJelZvVVhEc0JPeU9nT1dFPQ==;MjMwMDg4M0AzMjMxMmUzMDJlMzBqVHNNeXI3c0ptelpOd0Fyb2lTNkpsVXY5WWd6N2ZFMlptU2tDVU96cXFvPQ==;MjMwMDg4NEAzMjMxMmUzMDJlMzBEaHFlTjZnLzBtbENhVi9aVlhSaW9lVk5vc2RHNVlqSlZsRmptcXlodnhVPQ==;MjMwMDg4NUAzMjMxMmUzMDJlMzBaZ1RNenR0QVZVNEd2QVdYSThvd2d6aUwvNVYrQStrek9FTjRHeTlqZzk4PQ== ");

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
