using JobTrackingUI.Components;
using JobTrackingUI.Helpers;
using JobTrackingUI.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string webApiBaseUrl = builder.Configuration["WebApiBaseUrl"]
                       ?? throw new InvalidOperationException("WebApiBaseUrl is not configured.");

builder.Services.AddHttpClient<ApplicationService>(
    client => client.BaseAddress = new Uri(webApiBaseUrl));

builder.Services.AddHttpClient<EnumService>(
    client => client.BaseAddress = new Uri(webApiBaseUrl));

builder.Services.AddScoped<FileHelpers>();

builder.Services.AddMudServices();

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
