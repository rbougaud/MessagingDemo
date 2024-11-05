using Application;
using Hangfire;
using Infrastructure;
using OutboxHf.Middlewares;
using Presentation.Extensions;
using Presentation.Filters;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Filter.With<ApplicationLogWithRequestsFilter>()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

try
{
    Log.Logger.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddUserSecrets<Program>();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, cfg) =>
    {
        cfg.Enrich.FromLogContext()
            .Filter.With<ApplicationLogWithRequestsFilter>()
            .WriteTo.Console()
            .MinimumLevel.Debug();
    });
    builder.Services.AddControllers()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressInferBindingSourcesForParameters = true;
                    });
    builder.Services.AddHttpClient().AddDistributedMemoryCache();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration["ConnectionString"]!);
    builder.Services.ConfigurationMassTransit();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", builder =>
        {
            builder.WithOrigins("http://localhost:4200") // L'URL frontend Angular
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    app.UseBackgroundJobs();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
    }
    else
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = [] });
    }
    app.UseStaticFiles();
    app.UseCors("AllowFrontend");
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllers();
    app.UseHttpsRedirection();
    app.UseMiddleware<UserContextMiddleware>();
    Log.Logger.Information("App is running");
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex.Message);
}
finally
{
    Log.CloseAndFlush();
}