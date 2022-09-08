using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot()
    .AddCacheManager(s => s.WithDictionaryHandle());

builder.Logging
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();