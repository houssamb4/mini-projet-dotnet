var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();  // Adding MVC support
builder.Services.AddRazorPages();  // Retaining Razor Pages support

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable routing for both MVC and Razor Pages
app.UseRouting();

// Enable authorization if needed
app.UseAuthorization();

// Map MVC controllers and Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");  // MVC default route

app.MapRazorPages();  // Razor Pages route

app.Run();
