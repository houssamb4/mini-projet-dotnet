var builder = WebApplication.CreateBuilder(args);

// Add necessary services for controllers (API) and Razor Pages (if needed)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure exception handling for different environments
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Developer exception page in development
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTPS Strict Transport Security in production
}

// Apply middleware
app.UseHttpsRedirection(); // Ensure HTTPS usage
app.UseStaticFiles(); // Serve static files (for views if needed)
app.UseRouting(); // Enable routing
app.UseAuthorization(); // Enable authorization

// Map API controller route for Reservation API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Map a test route for database connection (if needed)
app.MapControllerRoute(
    name: "testConnection",
    pattern: "test-db-connection",
    defaults: new { controller = "Database", action = "TestConnection" }
);

// Map Razor Pages if needed
app.MapRazorPages();

app.Run();
