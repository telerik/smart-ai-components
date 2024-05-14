using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TelerikBlazorSmartAIComponents.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTelerikBlazor();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<HttpClient>();

await builder.Build().RunAsync();
