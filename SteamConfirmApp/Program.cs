using SteamConfirmApp;
using SteamConfirmApp.Components;
using SteamConfirmApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// User Settings
builder.Services.AddCascadingValue(sp => new Settings());
// Steam Guard Timer
builder.Services.AddTransient(sp => new SteamGuardTimerService(TimeSpan.FromSeconds(1)));
// Steam Trades Timer
builder.Services.AddTransient(sp => new SteamTradesTimerService(TimeSpan.FromSeconds(5)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();